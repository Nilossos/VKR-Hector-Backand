using Backand.DbEntites;

namespace Backand.ManagersClasses
{
    public static class DeliveryAbilityManagers
    {
        public static async Task GetAllDeliveryAbility(HttpContext context)
        {

            List<DeliveryAbility> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.DeliveryAbility.ToList();
            await context.Response.WriteAsJsonAsync(list);
        }

        //Get field by id 
        public static async Task GetDeliveryAbilityById(int id, HttpContext context)
        {
            List<DeliveryAbility> list;
            using (ApplicationContext db = new ApplicationContext())
                list = db.DeliveryAbility.ToList();
            DeliveryAbility item = list.FirstOrDefault((f) => f.DeliveryAbilityId == id);
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
        public static async Task CreateDeliveryAbility(HttpContext context)
        {
            List<DeliveryAbility> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.DeliveryAbility.ToList();
                DeliveryAbility item = await context.Request.ReadFromJsonAsync<DeliveryAbility>();

                if (item != null)
                {
                    db.DeliveryAbility.Add(item);
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
        public static async Task UpdateDeliveryAbility(HttpContext context)
        {
            List<DeliveryAbility> list;
            DeliveryAbility DeliveryAbilityData = await context.Request.ReadFromJsonAsync<DeliveryAbility>();
            if (DeliveryAbilityData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.DeliveryAbility.ToList();
                    var item = list.FirstOrDefault(c => c.DeliveryAbilityId == DeliveryAbilityData.StorageId);
                    if (item != null)
                    {
                        //item.Id = constructionUnitData.ConstructionUnitTypeId;

                        item.CompanyTransportId = DeliveryAbilityData.CompanyTransportId;
                        item.StorageId = DeliveryAbilityData.StorageId;
                        item.ObjectsId = DeliveryAbilityData.ObjectsId;
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
        public static async void DeleteDeliveryAbility(HttpContext context, int id)
        {
            List<DeliveryAbility> list;
            // если пользователь найден, удаляем его
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.DeliveryAbility.ToList();
                DeliveryAbility item = list.FirstOrDefault((f) => f.DeliveryAbilityId == id);
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
