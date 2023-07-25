using Backand.DbEntites;

namespace Backand.ManagersClasses
{
    public static class CompanyTransportManagers
    {
        public static async Task GetAllCompanyTransport(HttpContext context)
        {
            List<CompanyTransport> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.CompanyTransport.ToList();
                await context.Response.WriteAsJsonAsync(list);
            }
        }

        public static async Task GetCompanyTransportById(HttpContext context, int id)
        {
            List<CompanyTransport> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.CompanyTransport.ToList();
                CompanyTransport item = list.FirstOrDefault((f) => f.CompanyTransportId == id);
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

        public static async Task CreateCompanyTransport(HttpContext context)
        {
            List<CompanyTransport> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.CompanyTransport.ToList();
                CompanyTransport item = await context.Request.ReadFromJsonAsync<CompanyTransport>();
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

        public static async Task UpdateCompanyTransport(HttpContext context, int id)
        {
            List<CompanyTransport> list;
            CompanyTransport CompanyTransportData = await context.Request.ReadFromJsonAsync<CompanyTransport>();
            if (CompanyTransportData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.CompanyTransport.ToList();
                    CompanyTransport item = list.FirstOrDefault(m => m.CompanyTransportId == CompanyTransportData.CompanyTransportId);
                    if (item != null)
                    {

                        item.TransportModeId = CompanyTransportData.TransportModeId;
                        item.CompanyId = CompanyTransportData.CompanyId;
                        item.BrandName = CompanyTransportData.BrandName;
                        item.ModelName = CompanyTransportData.ModelName;
                        item.CoefficientTypeId = CompanyTransportData.CoefficientTypeId;
                        item.CoefficientValue = CompanyTransportData.CoefficientValue;
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
        public static async Task DeleteCompanyTransport(HttpContext context, int id)
        {
            List<CompanyTransport> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.CompanyTransport.ToList();
                var item = list.FirstOrDefault(m => m.CompanyTransportId == id);
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
