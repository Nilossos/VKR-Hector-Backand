using Backand.DbEntites;
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
        public static async Task GetTest(HttpContext context)
        {

        }
        //Get all fields
        public static async Task<IResult> GetAllMines(ApplicationContext context)
        {
           
            var mines = await Task.Run(() => context.Mine.Select(mine => new MineLink
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

        //Get field by id 
        public static async Task<IResult> GetMineById(int id, ApplicationContext dbContext)
        {
            return await Task.Run(() =>
            {
                Mine mine=dbContext.Mine.First(m => m.MineId == id);
                MineInfo info = new(dbContext,mine);
                return Results.Json(info);
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

