using Backand.DbEntities;

namespace Backand.ManagersClasses
{
    public static class CoefficientTypeManagers
    {
        public static async Task GetAllCoefficientType(HttpContext context)
        {
            List<CoefficientType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.CoefficientType.ToList();
                await context.Response.WriteAsJsonAsync(list);
            }
        }

        public static async Task GetCoefficientTypeById(HttpContext context, int id)
        {
            List<CoefficientType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.CoefficientType.ToList();
                CoefficientType item = list.FirstOrDefault((f) => f.CoefficientTypeId == id);
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

        public static async Task CreateCoefficientType(HttpContext context)
        {
            List<CoefficientType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.CoefficientType.ToList();
                CoefficientType item = await context.Request.ReadFromJsonAsync<CoefficientType>();
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

        public static async Task UpdateCoefficientType(HttpContext context, int id)
        {
            List<CoefficientType> list;
            CoefficientType CoefficientTypeData = await context.Request.ReadFromJsonAsync<CoefficientType>();
            if (CoefficientTypeData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.CoefficientType.ToList();
                    CoefficientType item = list.FirstOrDefault(m => m.CoefficientTypeId == CoefficientTypeData.CoefficientTypeId);
                    if (item != null)
                    {

                        item.Name = CoefficientTypeData.Name;
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
        public static async Task DeleteCoefficientType(HttpContext context, int id)
        {
            List<CoefficientType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.CoefficientType.ToList();
                var item = list.FirstOrDefault(m => m.CoefficientTypeId == id);
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
