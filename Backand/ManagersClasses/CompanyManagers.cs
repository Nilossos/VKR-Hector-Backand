using Backand.DbEntities;

namespace Backand.ManagersClasses
{
    public class CompanyManagers
    {
        public static async Task GetAllCompany(HttpContext context)
        {
            List<Company> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.Company.ToList();
                await context.Response.WriteAsJsonAsync(list);
            }
        }

        public static async Task GetCompanyById(HttpContext context, int id)
        {
            List<Company> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.Company.ToList();
                Company item = list.FirstOrDefault((f) => f.CompanyId == id);
                if (item != null)
                {
                    await context.Response.WriteAsJsonAsync(item);
                }
                else
                {
                    await context.Response.WriteAsJsonAsync("Item is null");
                }
            }
        }

        public static async Task CreateCompany(HttpContext context)
        {
            List<Company> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.Company.ToList();
                Company item = await context.Request.ReadFromJsonAsync<Company>();
                if (item != null)
                {
                    list.Add(item);
                    await db.SaveChangesAsync();
                    await context.Response.WriteAsJsonAsync(list);
                }
                else
                {
                    await context.Response.WriteAsJsonAsync("Item is null");
                }
            }
        }

        public static async Task UpdateCompany(HttpContext context, int id)
        {
            List<Company> list;
            Company companyData = await context.Request.ReadFromJsonAsync<Company>();
            if (companyData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.Company.ToList();
                    Company item = list.FirstOrDefault(m => m.CompanyId == companyData.CompanyId);
                    if (item != null)
                    {
                        item.CompanyTypeId = companyData.CompanyTypeId;
                        item.PhoneNumber = companyData.PhoneNumber;
                        item.Url = companyData.Url;
                        item.Email = companyData.Email;
                        list.Add(item);
                        await db.SaveChangesAsync();
                        await context.Response.WriteAsJsonAsync(list);
                    }
                    else
                    {
                        await context.Response.WriteAsJsonAsync("Item is null");
                    }
                }
            }
            else
            {
                await context.Response.WriteAsJsonAsync("Manufacture is null");
            }
        }
        public static async Task DeleteCompany(HttpContext context, int id)
        {
            List<Company> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.Company.ToList();
                var item = list.FirstOrDefault(m => m.CompanyId == id);
                if (item != null)
                {
                    list.Remove(item);
                    await db.SaveChangesAsync();
                    await context.Response.WriteAsJsonAsync(list);
                }
            }
        }
    }
}
