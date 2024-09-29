using Backand.DbEntities;
using System.Xml.Linq;
using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses
{
    class FrontendMine
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class FieldManagers
    {
        //Get all fields
        public static async Task<IResult> GetAllMines(ApplicationContext context)
        {
            var data = context.Mine.Select(m=>m.MineId);

            var mines = await Task.Run(() => context.Mine.
                Select(
                    mine => new MineLink
                    {
                        Id=mine.MineId,
                        Name=mine.Name,
                        Coordination=mine.Center,
                        Objects = context.Objects.Where(o=>o.MineId==mine.MineId).
                            Select(o => new MapLink 
                            {
                                Id=o.ObjectsId,
                                Name=o.Name, 
                                Coordination=o.Spot
                            }).ToArray()
                    }));
            return Results.Json(mines);
        }
        public class MineDto
        {
            public Mine Mine { get; set; }
            public bool IsAssemblyShop { get; set; }
        }
        //Get field by id 
        public static async Task<IResult> GetMineById(int id, ApplicationContext dbContext)
        {
            Mine mine = await dbContext.Mine
                .Include(m => m.Objects)
                .ThenInclude(o => o.Constructions)
                .ThenInclude(c => c.ConstructionType)
                .FirstOrDefaultAsync(m => m.MineId == id);
            await dbContext.Entry(mine).Reference(m => m.Subsidiary).LoadAsync();
            bool isAssemblyShop = mine.Objects
                .SelectMany(o => o.Constructions)
                .Any(s => s.ConstructionType.IsAssemblyShop);
            return Results.Json(new MineDto
            {
                Mine = mine,
                IsAssemblyShop = isAssemblyShop
            });
        }

        //Create new field 
        public static async Task CreateMine(HttpContext context)
        {
            List<Mine> mines;
            using (ApplicationContext db = new ApplicationContext())
            {
                mines = db.Mine.ToList();
                Mine mine = await context.Request.ReadFromJsonAsync<Mine>();

                if (mine != null)
                {
                    db.Mine.Add(mine);
                    await db.SaveChangesAsync(); // Save changes to the database
                    await context.Response.WriteAsJsonAsync(mine);
                }
                else
                {
                    await context.Response.WriteAsJsonAsync("Mine hasn't enough parameters");
                }
            }
        }

        //Update fields
        public static async Task UpdateMine(HttpContext context)
        {
            List<Mine> mines;
            Mine mineData = await context.Request.ReadFromJsonAsync<Mine>();
            if (mineData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    
                    var mine = db.Mine.FirstOrDefault(field => field.MineId == mineData.MineId);
                    if (mine != null)
                    {
                        mine.Name = mineData.Name;
                        //mine.Coordinates = mineData.Coordinates;
                        await db.SaveChangesAsync();
                        await context.Response.WriteAsJsonAsync(mine);
                    }

                    else
                    {
                        await context.Response.WriteAsJsonAsync("Field is null");
                    }
                }
            }
        }

        //Delete field 
        public static async Task<IResult> DeleteMine(ApplicationContext dbContext, int id)
        {
            List<Mine> mines;
            // если пользователь найден, удаляем его
            
            mines = dbContext.Mine.ToList();
            Mine? mine = mines.FirstOrDefault((f) => f.MineId == id);
            if (mine != null)
            {
                mines.Remove(mine);
                return Results.Json(mines);

            }
            // если не найден, отправляем статусный код и сообщение об ошибке
            else
            {
                return Results.Json(new {Error=true, Message = $"Ошибка! Месторождение с {id} не найдено!" });
            }
            
        }
    }
}

