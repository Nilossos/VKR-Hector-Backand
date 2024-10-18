using Backand.DbEntities;
using Backand.FrontendEntities;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses
{
    public class ObjectManagers
    {
        //Get all objects
        public static async Task GetAllObjects(HttpContext context)
        {

            List<ObjectEntity> objects;
            using (ApplicationContext db = new ApplicationContext())
                objects = db.Object.ToList();
            await context.Response.WriteAsJsonAsync(objects);
        }

        //Get object by id 
        public static async Task GetObjectById(int id, HttpContext context)
        {
            List<ObjectEntity> objects;
            using (ApplicationContext db = new ApplicationContext())
                objects = db.Object.ToList();
            ObjectEntity object1 = objects.FirstOrDefault((f) => f.ObjectId == id);
            if (object1 != null)
                await context.Response.WriteAsJsonAsync(object1);
            else
                await context.Response.WriteAsJsonAsync("Object is null");
        }

        //Get objects by mine id
        public static async Task<IResult> GetObjectsByMineId(int mine_id, ApplicationContext appContext)
        {
            var res = await Task.Run(() => appContext.Object.
                Where(o => o.MineId == mine_id).
                Select(o => o.Link).
                ToList());
            return Results.Json(res);
        }

        //Create new object 
        public static async Task CreateObject(HttpContext context)
        {
            List<ObjectEntity> objects;
            using (ApplicationContext db = new ApplicationContext())
            {
                objects = db.Object.ToList();
                ObjectEntity object1 = await context.Request.ReadFromJsonAsync<ObjectEntity>();
                //int newId;
                if (object1 != null)
                {

                    //object1.ObjectsId = newId;

                    objects.Add(object1);
                    await db.SaveChangesAsync();
                    await context.Response.WriteAsJsonAsync(object1);
                }
                else
                {
                    await context.Response.WriteAsJsonAsync("Object hasn't enought parameters");
                }
            }
        }

        //Update object
        public static async Task UpdateObject(HttpContext context)
        {
            ObjectEntity objectData = await context.Request.ReadFromJsonAsync<ObjectEntity>();
            if (objectData != null)
            {
                List<ObjectEntity> objects;
                using (ApplicationContext db = new ApplicationContext())
                {
                    objects = db.Object.ToList();
                    var object1 = objects.FirstOrDefault(o => o.ObjectId == objectData.ObjectId);
                    if (object1 != null)
                    {
                        object1.Name = objectData.Name;
                        object1.Coordinates = objectData.Coordinates;
                        object1.MineId = objectData.MineId;
                        object1.ContainsAssemblyShop = objectData.ContainsAssemblyShop;
                        await db.SaveChangesAsync();
                        await context.Response.WriteAsJsonAsync(object1);
                    }
                    else
                    {
                        await context.Response.WriteAsJsonAsync("Object is null");
                    }
                }
            }
        }

        //Delete object 
        public static async void DeleteObject(HttpContext context, int id)
        {
            List<ObjectEntity> objects;
            using (ApplicationContext db = new ApplicationContext())
            {
                objects = db.Object.ToList();
                ObjectEntity object1 = objects.FirstOrDefault((o) => o.ObjectId == id);
                // если object1 найден, удаляем его
                if (object1 != null)
                {
                    objects.Remove(object1);
                    await db.SaveChangesAsync();
                    await context.Response.WriteAsJsonAsync(object1);

                }
                // если не найден, отправляем статусный код и сообщение об ошибке
                else
                {
                    await context.Response.WriteAsJsonAsync("Object doen't exist");
                }
            }
        }
    }
}
