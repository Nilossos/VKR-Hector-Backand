using Backand.DbEntities;
using Backand.FrontendEntities;

namespace Backand.ManagersClasses
{
    public class ObjectsManagers
    {
        //Get all objects
        public static async Task GetAllObjects(HttpContext context)
        {

            List<Objects> objects;
            using (ApplicationContext db = new ApplicationContext())
                objects = db.Objects.ToList();
            await context.Response.WriteAsJsonAsync(objects);
        }

        //Get object by id 
        public static async Task GetObjectById(int id, HttpContext context)
        {
            List<Objects> objects;
            using (ApplicationContext db = new ApplicationContext())
                objects = db.Objects.ToList();
            Objects object1 = objects.FirstOrDefault((f) => f.ObjectsId == id);
            if (object1 != null)
                await context.Response.WriteAsJsonAsync(object1);
            else
                await context.Response.WriteAsJsonAsync("Object is null");
        }

        //Get objects by mine id
        public static async Task<IResult> GetObjectsByMineId(int mine_id, ApplicationContext appContext)
        {
            var res = await Task.Run(() => appContext.Objects.
                Where(o => o.MineId == mine_id).
                Select(o => o.Link).
                ToList());
            return Results.Json(res);
        }

        //Create new object 
        public static async Task CreateObject(HttpContext context)
        {
            List<Objects> objects;
            using (ApplicationContext db = new ApplicationContext())
            {
                objects = db.Objects.ToList();
                Objects object1 = await context.Request.ReadFromJsonAsync<Objects>();
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
            Objects objectData = await context.Request.ReadFromJsonAsync<Objects>();
            if (objectData != null)
            {
                List<Objects> objects;
                using (ApplicationContext db = new ApplicationContext())
                {
                    objects = db.Objects.ToList();
                    var object1 = objects.FirstOrDefault(o => o.ObjectsId == objectData.ObjectsId);
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
            List<Objects> objects;
            using (ApplicationContext db = new ApplicationContext())
            {
                objects = db.Objects.ToList();
                Objects object1 = objects.FirstOrDefault((o) => o.ObjectsId == id);
                // если пользователь найден, удаляем его
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
