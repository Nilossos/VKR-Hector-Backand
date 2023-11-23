using Backand.DbEntities;
using System.Xml.Linq;

namespace Backand.ManagersClasses
{
    public static class ConstructionUnitManagers
    {
        public static async Task GetAllConstructionUnit(HttpContext context)
        {

            List<ConstructionUnit> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.ConstructionUnit.ToList();
            await context.Response.WriteAsJsonAsync(list);
        }

        //Get field by id 
        public static async Task GetConstructionUnitById(int id, HttpContext context)
        {
            List<ConstructionUnit> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.ConstructionUnit.ToList();
            ConstructionUnit item = list.FirstOrDefault((f) => f.ConstructionUnitId == id);
            if (item != null)
            {
                await context.Response.WriteAsJsonAsync(item);
            }
            else
            {
                await context.Response.WriteAsJsonAsync("Item is null");
            }
        }

        //Create new field 
        public static async Task CreateConstructionUnit(HttpContext context)
        {
            List<ConstructionUnit> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.ConstructionUnit.ToList();
                ConstructionUnit item = await context.Request.ReadFromJsonAsync<ConstructionUnit>();

                if (item != null)
                {
                    db.ConstructionUnit.Add(item);
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
        public static async Task UpdateConstructionUnit(HttpContext context)
        {
            List<ConstructionUnit> list;
            ConstructionUnit constructionUnitData = await context.Request.ReadFromJsonAsync<ConstructionUnit>();
            if (constructionUnitData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.ConstructionUnit.ToList();
                    var item = list.FirstOrDefault(c => c.ConstructionUnitId == constructionUnitData.ConstructionUnitId);
                    if (item != null)
                    {
                        item.Name = constructionUnitData.Name;
                        item.ConstructionUnitTypeId = constructionUnitData.ConstructionUnitTypeId;
                        item.MeasureUnit = constructionUnitData.MeasureUnit;
                        list.Add(item);
                        await db.SaveChangesAsync();
                        await context.Response.WriteAsJsonAsync(list);
                    }
                }
            }
            else
            {
                await context.Response.WriteAsJsonAsync("Null");
            }
        }
        //Delete field 
        public static async void DeleteConstructionUnit(HttpContext context, int id)
        {
            List<ConstructionUnit> list;
            // если пользователь найден, удаляем его
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.ConstructionUnit.ToList();
                ConstructionUnit? item = list.FirstOrDefault((f) => f.ConstructionUnitId == id);
                if (item != null)
                {
                    list.Remove(item);
                    await db.SaveChangesAsync();
                    await context.Response.WriteAsJsonAsync(item);

                }
                // если не найден, отправляем статусный код и сообщение об ошибке
                else
                {
                    await context.Response.WriteAsJsonAsync("Item doen't exist");
                }
            }
        }
    }
}
