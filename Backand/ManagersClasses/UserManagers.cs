using Backand.DbEntites;

namespace Backand.ManagersClasses
{
    public static class UserManagers
    {

        public static async Task GetAllUser(HttpContext context)
        {

            List<User> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.User.ToList();
            await context.Response.WriteAsJsonAsync(list);
        }

        //Get field by id 
        public static async Task GetUserById(int id, HttpContext context)
        {
            List<User> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.User.ToList();
            User item = list.FirstOrDefault((f) => f.UserId == id);
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
        public static async Task CreateUser(HttpContext context)
        {
            List<User> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.User.ToList();
                User item = await context.Request.ReadFromJsonAsync<User>();

                if (item != null)
                {
                    db.User.Add(item);
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
        public static async Task UpdateUser(HttpContext context)
        {
            List<User> list;
            User UserData = await context.Request.ReadFromJsonAsync<User>();
            if (UserData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.User.ToList();
                    var item = list.FirstOrDefault(c => c.UserTypeId == UserData.UserTypeId);
                    if (item != null)
                    {
                        item.Surname = UserData.Surname;
                        item.PhoneNumber = UserData.PhoneNumber;
                        item.Photo = UserData.Photo;
                        //item.Post = UserData.Post;
                        item.BirthDate = UserData.BirthDate;
                        item.FirstName = UserData.FirstName;
                        item.UserTypeId = UserData.UserTypeId;
                        item.Token = UserData.Token;
                        item.Patronymic = UserData.Patronymic;

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
        public static async void DeleteUser(HttpContext context, int id)
        {
            List<User> list;
            // если пользователь найден, удаляем его
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.User.ToList();
                User item = list.FirstOrDefault((f) => f.UserId == id);
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
