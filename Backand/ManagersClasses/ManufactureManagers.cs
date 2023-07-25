using Backand.DbEntites;

namespace Backand.ManagersClasses
{
    public static class ManufactureManagers
    {
        public static async Task GetAllManufacture(HttpContext context)
        {
            List<Manufacturer> list;
            using(ApplicationContext db = new ApplicationContext())
            {
                list = db.Manufacturer.ToList();
                await context.Response.WriteAsJsonAsync(list);
            }
        }

        public static async Task GetManufactureById(HttpContext context, int id)
        {
            List<Manufacturer> list;
            using(ApplicationContext db = new ApplicationContext())
            {
                list = db.Manufacturer.ToList();
                Manufacturer item = list.FirstOrDefault((f) => f.ManufacturerId == id);
                if(item != null)
                {
                    await context.Response.WriteAsJsonAsync(item);
                }
                else
                {
                    await context.Response.WriteAsJsonAsync("Item is null");
                }
            }
        }

        public static async Task CreateManufacture(HttpContext context)
        {
            List<Manufacturer> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.Manufacturer.ToList();
                Manufacturer item = await context.Request.ReadFromJsonAsync<Manufacturer>();
                if(item != null)
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

        public static async Task UpdateManufacture(HttpContext context, int id)
        {
            List<Manufacturer> list;
            Manufacturer manufactureData = await context.Request.ReadFromJsonAsync<Manufacturer>();
            if(manufactureData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.Manufacturer.ToList();
                    Manufacturer item = list.FirstOrDefault(m => m.ManufacturerId == manufactureData.ManufacturerId);
                    if(item != null)
                    {
                        item.Name = manufactureData.Name;
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
        public static async Task DeleteManufacture(HttpContext context, int id)
        {
            List<Manufacturer> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.Manufacturer.ToList();
                var item = list.FirstOrDefault(m => m.ManufacturerId == id);
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
