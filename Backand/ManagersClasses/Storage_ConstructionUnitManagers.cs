using Backand.DbEntites;

namespace Backand.ManagersClasses
{
    public class Storage_ConstructionUnitManagers
    {
        public static async Task GetAllStorage_ConstructionUnit(HttpContext context)
        {

            List<Storage_ConstructionUnit> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.Storage_ConstructionUnit.ToList();
            await context.Response.WriteAsJsonAsync(list);
        }

        //Get field by id 
        public static async Task GetStorage_ConstructionUnitById(int id, HttpContext context)
        {
            List<Storage_ConstructionUnit> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.Storage_ConstructionUnit.ToList();
            Storage_ConstructionUnit item = list.FirstOrDefault((f) => f.Storage_ConstructionUnitId == id);
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
        public static async Task CreateStorage_ConstructionUnit(HttpContext context)
        {
            List<Storage_ConstructionUnit> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.Storage_ConstructionUnit.ToList();
                Storage_ConstructionUnit item = await context.Request.ReadFromJsonAsync<Storage_ConstructionUnit>();

                if (item != null)
                {
                    db.Storage_ConstructionUnit.Add(item);
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
        public static async Task UpdateStorage_ConstructionUnit(HttpContext context)
        {
            List<Storage_ConstructionUnit> list;
            Storage_ConstructionUnit Storage_ConstructionUnitData = await context.Request.ReadFromJsonAsync<Storage_ConstructionUnit>();
            if (Storage_ConstructionUnitData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.Storage_ConstructionUnit.ToList();
                    var item = list.FirstOrDefault(c => c.StorageId == Storage_ConstructionUnitData.StorageId);
                    if (item != null)
                    {
                        //item.Id = constructionUnitData.ConstructionUnitTypeId;

                        item.StorageId = Storage_ConstructionUnitData.StorageId;
                        item.ConstructionUnitId = Storage_ConstructionUnitData.ConstructionUnitId;
                        item.Amount = Storage_ConstructionUnitData.Amount;
                        item.Price = Storage_ConstructionUnitData.Price;
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
        public static async void DeleteStorage_ConstructionUnit(HttpContext context, int id)
        {
            List<Storage_ConstructionUnit> list;
            // если пользователь найден, удаляем его
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.Storage_ConstructionUnit.ToList();
                Storage_ConstructionUnit item = list.FirstOrDefault((f) => f.Storage_ConstructionUnitId == id);
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
