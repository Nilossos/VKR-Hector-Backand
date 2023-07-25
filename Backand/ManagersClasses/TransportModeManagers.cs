using Backand.DbEntites;

namespace Backand.ManagersClasses
{
    public static class TransportModeManagers
    {

        public static async Task GetAllTransportMode(HttpContext context)
        {
            List<TransportMode> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.TransportMode.ToList();
                await context.Response.WriteAsJsonAsync(list);
            }
        }

        public static async Task GetTransportModeById(HttpContext context, int id)
        {
            List<TransportMode> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.TransportMode.ToList();
                TransportMode item = list.FirstOrDefault((f) => f.TransportModeId == id);
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

        public static async Task CreateTransportMode(HttpContext context)
        {
            List<TransportMode> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.TransportMode.ToList();
                TransportMode item = await context.Request.ReadFromJsonAsync<TransportMode>();
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

        public static async Task UpdateTransportMode(HttpContext context, int id)
        {
            List<TransportMode> list;
            TransportMode TransportModeData = await context.Request.ReadFromJsonAsync<TransportMode>();
            if (TransportModeData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.TransportMode.ToList();
                    TransportMode item = list.FirstOrDefault(m => m.TransportModeId == TransportModeData.TransportTypeId);
                    if (item != null)
                    {

                        item.Name = TransportModeData.Name;
                        item.AvgSpeed = TransportModeData.AvgSpeed;
                        item.TransportTypeId = TransportModeData.TransportTypeId;
                       
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
        public static async Task DeleteTransportMode(HttpContext context, int id)
        {
            List<TransportMode> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.TransportMode.ToList();
                var item = list.FirstOrDefault(m => m.TransportModeId == id);
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
