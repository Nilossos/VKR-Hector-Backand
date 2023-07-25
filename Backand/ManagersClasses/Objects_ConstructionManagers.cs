using Backand.DbEntites;
using System.Xml.Linq;

namespace Backand.ManagersClasses
{
    public class Objects_ConstructionManagers
    {
        //Get all fields
        public static async Task GetAllObjects_Construction(HttpContext context)
        {

            List<Objects_Construction> objectsConstructionList;
            using (ApplicationContext db = new ApplicationContext())
                objectsConstructionList = db.Objects_Construction.ToList();
            await context.Response.WriteAsJsonAsync(objectsConstructionList);
        }

        //Get field by id 
        public static async Task GetObjects_ConstructionById(int id, HttpContext context)
        {
            List<Objects_Construction> objectsConstructionList;
            using (ApplicationContext db = new ApplicationContext())
                objectsConstructionList = db.Objects_Construction.ToList();
            Objects_Construction objects_Construction = objectsConstructionList.FirstOrDefault((o) => o.ConstructionId == id);
            if (objects_Construction != null)
            {
                await context.Response.WriteAsJsonAsync(objects_Construction);
            }
            else
            {
                await context.Response.WriteAsJsonAsync("Objects Construction is null");
            }
        }

        //Create new field 
        public static async Task CreateObjects_Construction(HttpContext context)
        {
            List<Objects_Construction> objectsConstructionList;
            using (ApplicationContext db = new ApplicationContext())
            {
                objectsConstructionList = db.Objects_Construction.ToList();
                Objects_Construction objects_Construction = await context.Request.ReadFromJsonAsync<Objects_Construction>();

                if (objects_Construction != null)
                {

                    objectsConstructionList.Add(objects_Construction);
                    await db.SaveChangesAsync();
                    await context.Response.WriteAsJsonAsync(objects_Construction);
                }
                else
                {
                    await context.Response.WriteAsJsonAsync("Objects Construction hasn't enought parameters");
                }
            }
        }

        //Update fields
        public static async Task UpdateObjects_Construction(HttpContext context)
        {
            Objects_Construction object_constructionData = await context.Request.ReadFromJsonAsync<Objects_Construction>();
            if (object_constructionData != null)
            {
                List<Objects_Construction> objects_ConstructionList;
                using (ApplicationContext db = new ApplicationContext())
                {
                    objects_ConstructionList = db.Objects_Construction.ToList();
                    var object_construction = objects_ConstructionList.FirstOrDefault(oc => oc.ConstructionId == object_constructionData.ConstructionId);
                    if (object_construction != null)
                    {
                        object_construction.ConstructionId = object_constructionData.ConstructionId;
                        object_construction.ObjectsId = object_constructionData.ObjectsId;
                        await db.SaveChangesAsync();
                        await context.Response.WriteAsJsonAsync(object_construction);
                    }

                    else
                    {
                        await context.Response.WriteAsJsonAsync("Object Construction is null");
                    }
                }
            }
        }
        //Delete field 
        public static async void DeleteObjects_Construction(HttpContext context, int id)
        {
            List<Objects_Construction> objects_ConstructionsList;
            using (ApplicationContext db = new ApplicationContext())
            {
                objects_ConstructionsList = db.Objects_Construction.ToList();
                Objects_Construction? objects_Construction = objects_ConstructionsList.FirstOrDefault((oc) => oc.ConstructionId == id);
                // если пользователь найден, удаляем его
                if (objects_Construction != null)
                {
                    objects_ConstructionsList.Remove(objects_Construction);
                    await db.SaveChangesAsync();
                    await context.Response.WriteAsJsonAsync(objects_Construction);

                }
                // если не найден, отправляем статусный код и сообщение об ошибке
                else
                {
                    await context.Response.WriteAsJsonAsync("Object construction doen't exist");
                }
            }
        }
    }
}
