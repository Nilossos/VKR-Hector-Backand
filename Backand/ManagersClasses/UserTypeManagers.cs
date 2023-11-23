using Backand.DbEntities;

namespace Backand.ManagersClasses
{
    public static class UserTypeManagers
    {
        public static async Task GetAllUserType(HttpContext context)
        {

            List<UserType> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.UserType.ToList();
            await context.Response.WriteAsJsonAsync(list);
        }

        //Get field by id 
        public static async Task GetUserTypeById(int id, HttpContext context)
        {
            List<UserType> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.UserType.ToList();
            UserType item = list.FirstOrDefault((f) => f.UserTypeId == id);
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
        public static async Task CreateUserType(HttpContext context)
        {
            List<UserType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.UserType.ToList();
                UserType item = await context.Request.ReadFromJsonAsync<UserType>();

                if (item != null)
                {
                    db.UserType.Add(item);
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
        public static async Task UpdateUserType(HttpContext context)
        {
            List<UserType> list;
            UserType UserTypeData = await context.Request.ReadFromJsonAsync<UserType>();
            if (UserTypeData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.UserType.ToList();
                    var item = list.FirstOrDefault(c => c.UserTypeId == UserTypeData.UserTypeId);
                    if (item != null)
                    {
                        item.Name = UserTypeData.Name;
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
        public static async void DeleteUserType(HttpContext context, int id)
        {
            List<UserType> list;
            // если пользователь найден, удаляем его
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.UserType.ToList();
                UserType item = list.FirstOrDefault((f) => f.UserTypeId == id);
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
