/*using Backand.DbEntites;

namespace Backand.ManagersClasses
{
    public class ContactInfoTypeManagers
    {
        public static async Task GetAllContactInfoType(HttpContext context)
        {
            List<ContactInfoType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.ContactInfoType.ToList();
                await context.Response.WriteAsJsonAsync(list);
            }
        }

        public static async Task GetContactInfoTypeById(HttpContext context, int id)
        {
            List<ContactInfoType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.ContactInfoType.ToList();
                ContactInfoType item = list.FirstOrDefault((f) => f.ContactInfoTypeId == id);
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

        public static async Task CreateContactInfoType(HttpContext context)
        {
            List<ContactInfoType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.ContactInfoType.ToList();
                ContactInfoType item = await context.Request.ReadFromJsonAsync<ContactInfoType>();
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

        public static async Task UpdateContactInfoType(HttpContext context, int id)
        {
            List<ContactInfoType> list;
            ContactInfoType contactInfoTypeData = await context.Request.ReadFromJsonAsync<ContactInfoType>();
            if (contactInfoTypeData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.ContactInfoType.ToList();
                    ContactInfoType item = list.FirstOrDefault(m => m.ContactInfoTypeId == contactInfoTypeData.ContactInfoTypeId);
                    if (item != null)
                    {
                        item.Name = contactInfoTypeData.Name;
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
        public static async Task DeleteContactInfoType(HttpContext context, int id)
        {
            List<ContactInfoType> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.ContactInfoType.ToList();
                var item = list.FirstOrDefault(m => m.ContactInfoTypeId == id);
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
*/