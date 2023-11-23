using Backand.DbEntities;

namespace Backand.ManagersClasses
{
    public static class TransportFleetManagers
    {
        public static async Task GetAllTransportFleet(HttpContext context)
        {
            List<TransportFleet> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.TransportFleet.ToList();
                await context.Response.WriteAsJsonAsync(list);
            }
        }

        public static async Task GetTransportFleetById(HttpContext context, int id)
        {
            List<TransportFleet> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.TransportFleet.ToList();
                TransportFleet item = list.FirstOrDefault((f) => f.TransportFleetId == id);
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

        public static async Task CreateTransportFleet(HttpContext context)
        {
            List<TransportFleet> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.TransportFleet.ToList();
                TransportFleet item = await context.Request.ReadFromJsonAsync<TransportFleet>();
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

        public static async Task UpdateTransportFleet(HttpContext context, int id)
        {
            List<TransportFleet> list;
            TransportFleet TransportFleetData = await context.Request.ReadFromJsonAsync<TransportFleet>();
            if (TransportFleetData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.TransportFleet.ToList();
                    TransportFleet item = list.FirstOrDefault(m => m.TransportFleetId == TransportFleetData.TransportFleetId);
                    if (item != null)
                    {

                        item.Name = TransportFleetData.Name;
						item.Address = TransportFleetData.Address;
						item.CompanyId = TransportFleetData.CompanyId;
                        item.RegionId = TransportFleetData.RegionId;
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
        public static async Task DeleteTransportFleet(HttpContext context, int id)
        {
            List<TransportFleet> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.TransportFleet.ToList();
                var item = list.FirstOrDefault(m => m.TransportFleetId == id);
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
