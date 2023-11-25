using Backand.DbEntities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backand.ManagersClasses
{
    public class TransportTypeManagers
    {
        public static IResult GetAllTransportType(ApplicationContext dbContext)
        {
            var links=dbContext.TransportType.Select(tt => tt.Link);
            return Results.Json(links);
        }

        public static async Task GetTransportTypeById(HttpContext context, int id)
        {
            List<TransportType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.TransportType.ToList();
                TransportType item = list.FirstOrDefault((f) => f.TransportTypeId == id);
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

        public static async Task CreateTransportType(HttpContext context)
        {
            List<TransportType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.TransportType.ToList();
                TransportType item = await context.Request.ReadFromJsonAsync<TransportType>();
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

        public static async Task UpdateTransportType(HttpContext context, int id)
        {
            List<TransportType> list;
            TransportType transportTypeData = await context.Request.ReadFromJsonAsync<TransportType>();
            if (transportTypeData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.TransportType.ToList();
                    TransportType item = list.FirstOrDefault(m => m.TransportTypeId == transportTypeData.TransportTypeId);
                    if (item != null)
                    {

                        item.Name = transportTypeData.Name;
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
        public static async Task DeleteTransportType(HttpContext context, int id)
        {
            List<TransportType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.TransportType.ToList();
                var item = list.FirstOrDefault(m => m.TransportTypeId == id);
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
