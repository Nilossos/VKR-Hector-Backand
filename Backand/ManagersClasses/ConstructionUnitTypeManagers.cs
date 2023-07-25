using Backand.DbEntites;
using System.Xml.Linq;

namespace Backand.ManagersClasses
{
    public class ConstructionUnitTypeManagers
    {
        public static async Task GetAllConstructionUnitType(HttpContext context)
        {

            List<ConstructionUnitType> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.ConstructionUnitType.ToList();
            await context.Response.WriteAsJsonAsync(list);
        }

        //Get field by id 
        public static async Task GetByConstructionUnitTypeId(int id, HttpContext context)
        {
            List<ConstructionUnitType> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.ConstructionUnitType.ToList();
            ConstructionUnitType item = list.FirstOrDefault((c) => c.ConstructionUnitTypeId == id);
            if (item != null)
            {
                await context.Response.WriteAsJsonAsync(item);
            }
            else
            {
                await context.Response.WriteAsJsonAsync("ConstructionUnitType is null");
            }
        }

        //Create new field 
        public static async Task CreateConstructionUnitType(HttpContext context)
        {
            List<ConstructionUnitType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.ConstructionUnitType.ToList();
                ConstructionUnitType item = await context.Request.ReadFromJsonAsync<ConstructionUnitType>();

                if (item != null)
                {
                    db.ConstructionUnitType.Add(item);
                    await db.SaveChangesAsync(); // Save changes to the database
                    await context.Response.WriteAsJsonAsync(item);
                }
                else
                {
                    await context.Response.WriteAsJsonAsync("Item hasn't enough parameters");
                }
            }
        }

        //Update fields
        public static async Task UpdateConstructionUnitType(HttpContext context)
        {
            List<ConstructionUnitType> list;
            ConstructionUnitType constructionUnitTypeData = await context.Request.ReadFromJsonAsync<ConstructionUnitType>();
            if (constructionUnitTypeData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {

                    list = db.ConstructionUnitType.ToList();
                    var item = list.FirstOrDefault(f=> f.ConstructionUnitTypeId == constructionUnitTypeData.ConstructionUnitTypeId);
                    if (item != null)
                    {
                        item.Name = constructionUnitTypeData.Name;
                        await db.SaveChangesAsync();
                        await context.Response.WriteAsJsonAsync(item);
                    }

                    else
                    {
                        await context.Response.WriteAsJsonAsync("Item is null");
                    }
                }
            }
        }

        //Delete field 
        public static async void DeleteConstructionUnitType(HttpContext context, int id)
        {
            /*   List<Mine> mines;
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
               }*/
            List<ConstructionUnitType> list;
            using(ApplicationContext db = new ApplicationContext())
            {
                list = db.ConstructionUnitType.ToList();
                var item = list.FirstOrDefault(c => c.ConstructionUnitTypeId == id);
                if (item != null)
                {
                    list.Remove(item);
                    await db.SaveChangesAsync();
                    await context.Response.WriteAsJsonAsync(list);
                }
                else
                {
                    await context.Response.WriteAsJsonAsync("Item is null");
                }
            }
        }
    }
}
