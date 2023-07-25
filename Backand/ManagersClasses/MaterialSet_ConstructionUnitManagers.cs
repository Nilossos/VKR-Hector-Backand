using Backand.DbEntites;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backand.ManagersClasses
{
    public static class MaterialSet_ConstructionUnitManagers
    {
        public static async Task GetAllMaterial_SetConstructionUnit(HttpContext context)
        {
            List<MaterialSet_ConstructionUnit> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.MaterialSet_ConstructionUnit.ToList();
                await context.Response.WriteAsJsonAsync(list);
            }
        }

        public static async Task GetMaterialSet_ConstructionUnitById(HttpContext context, int id)
        {
            List<MaterialSet_ConstructionUnit> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.MaterialSet_ConstructionUnit.ToList();
                MaterialSet_ConstructionUnit item = list.FirstOrDefault((f) => f.MaterialSet_ConstructionUnitId == id);
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

        public static async Task CreateMaterialSet_ConstructionUnit(HttpContext context)
        {
            List<MaterialSet_ConstructionUnit> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.MaterialSet_ConstructionUnit.ToList();
                MaterialSet_ConstructionUnit item = await context.Request.ReadFromJsonAsync<MaterialSet_ConstructionUnit>();
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

        public static async Task UpdateMaterialSet_ConstructionUnit(HttpContext context, int id)
        {
            List<MaterialSet_ConstructionUnit> list;
            MaterialSet_ConstructionUnit MaterialSet_ConstructionUnitData = await context.Request.ReadFromJsonAsync<MaterialSet_ConstructionUnit>();
            if (MaterialSet_ConstructionUnitData != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    list = db.MaterialSet_ConstructionUnit.ToList();
                    MaterialSet_ConstructionUnit item = list.FirstOrDefault(m => m.MaterialSet_ConstructionUnitId == MaterialSet_ConstructionUnitData.MaterialSet_ConstructionUnitId);
                    if (item != null)
                    {

                        item.MaterialSetId = MaterialSet_ConstructionUnitData.MaterialSetId;
                        item.ConstructionUnitId = MaterialSet_ConstructionUnitData.ConstructionUnitId;
                        item.Amount = MaterialSet_ConstructionUnitData.Amount;
                       
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
        public static async Task DeleteMaterialSet_ConstructionUnit(HttpContext context, int id)
        {
            List<MaterialSet_ConstructionUnit> list;
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.MaterialSet_ConstructionUnit.ToList();
                var item = list.FirstOrDefault(m => m.MaterialSet_ConstructionUnitId == id);
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
