using Backand.DbEntites;
using System.Data;
using System.Xml.Linq;

namespace Backand.ManagersClasses
{
    public class StorageManagers
    {
        public static async Task GetAllStorage(HttpContext context)
        {

            List<Storage> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.Storage.ToList();
            await context.Response.WriteAsJsonAsync(list);
        }

        //Get field by id 
        public static async Task GetStorageById(int id, HttpContext context)
        {
            List<Storage> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.Storage.ToList();
            Storage item = list.FirstOrDefault((f) => f.StorageId == id);
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
        public static async Task CreateStorage(HttpContext context)
        {
            List<Storage> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.Storage.ToList();
                Storage item = await context.Request.ReadFromJsonAsync<Storage>();

                if (item != null)
                {
                    db.Storage.Add(item);
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
        public static async Task UpdateStorage(HttpContext context)
        {
            List<Storage> list;
            Storage storageData = await context.Request.ReadFromJsonAsync<Storage>();
            if (storageData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.Storage.ToList();
                    var item = list.FirstOrDefault(c => c.StorageId == storageData.StorageId);
                    if (item != null)
                    {
                        //item.Id = constructionUnitData.ConstructionUnitTypeId;

                        item.Address = storageData.Address;
                        item.ManufacturerId = storageData.ManufacturerId;
                        item.Name = storageData.Name;
                        item.Coordinates = storageData.Coordinates;
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
        public static async void DeleteStorage(HttpContext context, int id)
        {
            List<Storage> list;
            // если пользователь найден, удаляем его
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.Storage.ToList();
                Storage item = list.FirstOrDefault((f) => f.StorageId == id);
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
