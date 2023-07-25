using Backand.DbEntites;
using System.Xml.Linq;

namespace Backand.ManagersClasses
{
    public class FieldManagers
    {
        //Get all fields
        public static async Task GetAllMines(HttpContext context)
        {

            List<Mine> mines;
            using (ApplicationContext db = new ApplicationContext())
                mines = db.Mine.ToList();
            await context.Response.WriteAsJsonAsync(mines);
        }

        //Get field by id 
        public static async Task GetMineById(int id, HttpContext context)
        {
            List<Mine> mines;
            using (ApplicationContext db = new ApplicationContext())
                mines = db.Mine.ToList();
            Mine mine = mines.FirstOrDefault((f) => f.MineId == id);
            if (mine != null)
            {
                await context.Response.WriteAsJsonAsync(mine);
            }
            else
            {
                await context.Response.WriteAsJsonAsync("Mine is null");
            }
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

                    mines = db.Mine.ToList();
                    var mine = mines.FirstOrDefault(field => field.MineId == mineData.MineId);
                    if (mine != null)
                    {
                        mine.Name = mineData.Name;
                        mine.Coordinates = mineData.Coordinates;
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
        public static async void DeleteMine(HttpContext context, int id)
        {
            List<Mine> mines;
            // если пользователь найден, удаляем его
            using (ApplicationContext db = new ApplicationContext())
            {
                mines = db.Mine.ToList();
                Mine? mine = mines.FirstOrDefault((f) => f.MineId == id);
                if (mine != null)
                {
                    mines.Remove(mine);
                    await db.SaveChangesAsync();
                    await context.Response.WriteAsJsonAsync(mine);

                }
                // если не найден, отправляем статусный код и сообщение об ошибке
                else
                {
                    await context.Response.WriteAsJsonAsync("Field doen't exist");
                }
            }
        }
    }
}

