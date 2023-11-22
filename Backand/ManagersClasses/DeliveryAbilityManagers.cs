using Backand.DbEntities;

namespace Backand.ManagersClasses
{
    public static class DeliveryRegionManagers
    {
        public static async Task GetAllDeliveryRegion(HttpContext context)
        {

            List<DeliveryRegion> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.DeliveryRegion.ToList();
            await context.Response.WriteAsJsonAsync(list);
        }

        //Get field by id 
        public static async Task GetDeliveryRegionById(int id, HttpContext context)
        {
            List<DeliveryRegion> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.DeliveryRegion.ToList();
            DeliveryRegion item = list.FirstOrDefault((f) => f.DeliveryRegionId == id);
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
        public static async Task CreateDeliveryRegion(HttpContext context)
        {
            List<DeliveryRegion> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.DeliveryRegion.ToList();
                DeliveryRegion item = await context.Request.ReadFromJsonAsync<DeliveryRegion>();

                if (item != null)
                {
                    db.DeliveryRegion.Add(item);
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
        public static async Task UpdateDeliveryRegion(HttpContext context)
        {
            List<DeliveryRegion> list;
            DeliveryRegion DeliveryRegionData = await context.Request.ReadFromJsonAsync<DeliveryRegion>();
            if (DeliveryRegionData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.DeliveryRegion.ToList();
                    var item = list.FirstOrDefault(c => c.DeliveryRegionId == DeliveryRegionData.DeliveryRegionId);
                    if (item != null)
                    {
                        //item.Id = constructionUnitData.ConstructionUnitTypeId;

                        item.TransportFleet_TransportId = DeliveryRegionData.TransportFleet_TransportId;
                        item.RegionId = DeliveryRegionData.RegionId;
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
        public static async void DeleteDeliveryRegion(HttpContext context, int id)
        {
            List<DeliveryRegion> list;
            // если пользователь найден, удаляем его
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.DeliveryRegion.ToList();
                DeliveryRegion item = list.FirstOrDefault((f) => f.DeliveryRegionId == id);
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
