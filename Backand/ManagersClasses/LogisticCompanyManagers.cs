using Backand.DbEntities;
using System.Xml.Linq;

namespace Backand.ManagersClasses
{
    public class LogisticCompanyManagers
    {
        public static async Task GetAllLogisticCompany(HttpContext context)
        {

            List<LogisticCompany> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.LogisticCompany.ToList();
            await context.Response.WriteAsJsonAsync(list);
        }

        //Get field by id 
        public static async Task GetLogisticCompanyById(int id, HttpContext context)
        {
            List<LogisticCompany> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.LogisticCompany.ToList();
            LogisticCompany item = list.FirstOrDefault((f) => f.LogisticCompanyId == id);
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
        public static async Task CreateLogisticCompany(HttpContext context)
        {
            List<LogisticCompany> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.LogisticCompany.ToList();
                LogisticCompany item = await context.Request.ReadFromJsonAsync<LogisticCompany>();

                if (item != null)
                {
                    db.LogisticCompany.Add(item);
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
        public static async Task UpdateLogisticCompany(HttpContext context)
        {
            List<LogisticCompany> list;
            LogisticCompany logisticCompanyData = await context.Request.ReadFromJsonAsync<LogisticCompany>();
            if (logisticCompanyData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.LogisticCompany.ToList();
                    var item = list.FirstOrDefault(c => c.LogisticCompanyId == logisticCompanyData.LogisticCompanyId);
                    if (item != null)
                    {
                        item.Name = logisticCompanyData.Name;
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
        public static async void DeleteLogisticCompany(HttpContext context, int id)
        {
            List<LogisticCompany> list;
            // если пользователь найден, удаляем его
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.LogisticCompany.ToList();
                LogisticCompany? item = list.FirstOrDefault((f) => f.LogisticCompanyId == id);
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
