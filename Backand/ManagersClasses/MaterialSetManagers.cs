using Backand.DbEntities;
using System.Xml.Linq;

namespace Backand.ManagersClasses
{
    public class MaterialSetManagers
    {
        //Get all fields
        public static async Task GetAllMaterialSet(HttpContext context)
        {

            List<MaterialSet> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.MaterialSet.ToList();
            await context.Response.WriteAsJsonAsync(list);
        }

        //Get field by id 
        public static async Task GetMaterialSetById(int id, HttpContext context)
        {
            List<MaterialSet> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.MaterialSet.ToList();
            MaterialSet item  = list.FirstOrDefault((f) => f.MaterialSetId == id);
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
        public static async Task CreateMaterialSet(HttpContext context)
        {
            List<MaterialSet> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.MaterialSet.ToList();
                MaterialSet item  = await context.Request.ReadFromJsonAsync<MaterialSet>();

                if (item != null)
                {
                    db.MaterialSet.Add(item);
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
        public static async Task UpdateMaterialSet(HttpContext context)
        {
            List<MaterialSet> list;
            MaterialSet materialSetData = await context.Request.ReadFromJsonAsync<MaterialSet>();
            if (materialSetData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {

                    list = db.MaterialSet.ToList();
                    var item = list.FirstOrDefault(field => field.MaterialSetId == materialSetData.MaterialSetId);
                    if (item != null)
                    {
                       /* item.MaterialSetId = mineData.Name;*/
                        item.ConstructionTypeId = materialSetData.ConstructionTypeId;
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
        public static async void DeleteMaterialSet(HttpContext context, int id)
        {
            List<MaterialSet> list;
            // если пользователь найден, удаляем его
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.MaterialSet.ToList();
                MaterialSet? item  = list.FirstOrDefault((f) => f.MaterialSetId == id);
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
