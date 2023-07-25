using Backand.DbEntites;
using System.Xml.Linq;

namespace Backand.ManagersClasses
{
    public class CompanyTypeManagers
    {

        public static async Task GetAllCompanyType(HttpContext context)
        {

            List<CompanyType> list;
            using (ApplicationContext db = new ApplicationContext())
            list = db.CompanyType.ToList();
            await context.Response.WriteAsJsonAsync(list);
        }

        //Get field by id 
        public static async Task GetCompanyTypeById(int id, HttpContext context)
        {
            List<CompanyType> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.CompanyType.ToList();
            CompanyType item = list.FirstOrDefault((f) => f.CompanyTypeId == id);
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
        public static async Task CreateCompanyType(HttpContext context)
        {
            List<CompanyType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.CompanyType.ToList();
                CompanyType item = await context.Request.ReadFromJsonAsync<CompanyType>();

                if (item != null)
                {
                    db.CompanyType.Add(item);
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
        public static async Task UpdateCompanyType(HttpContext context)
        {
            List<CompanyType> list;
            CompanyType companyTypeData = await context.Request.ReadFromJsonAsync<CompanyType>();
            if (companyTypeData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {

                    list = db.CompanyType.ToList();
                    var item = list.FirstOrDefault(field => field.CompanyTypeId == companyTypeData.CompanyTypeId);
                    if (item != null)
                    {
                        item.Name = companyTypeData.Name;
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
        public static async void DeleteCompanyType(HttpContext context, int id)
        {
            List<CompanyType> list;
            // если пользователь найден, удаляем его
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.CompanyType.ToList();
                CompanyType item = list.FirstOrDefault((f) => f.CompanyTypeId == id);
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
