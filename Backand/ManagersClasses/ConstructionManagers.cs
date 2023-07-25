using Backand.DbEntites;

namespace Backand.ManagersClasses
{

    public static class ConstructionManagers
        {
    //Get all construction
    public static async Task GetAllConstructions(HttpContext context)
    {

        List<Construction> constructions;
        using (ApplicationContext db = new ApplicationContext())
            constructions = db.Construction.ToList();
        await context.Response.WriteAsJsonAsync(constructions);
    }

    //Get object by id 
    public static async Task GetConstructionById(int id, HttpContext context)
    {
        List<Construction> constructions;
        using (ApplicationContext db = new ApplicationContext())
            constructions = db.Construction.ToList();
        Construction construction = constructions.FirstOrDefault((c) => c.ConstructionId == id);
        if (construction != null)
        {

            await context.Response.WriteAsJsonAsync(construction);
        }
        else
        {
            await context.Response.WriteAsJsonAsync("Construction  is null");
        }
    }

    //Create new object 
    public static async Task CreateConstruction(HttpContext context)
    {
        List<Construction> constructions;
           
            using (ApplicationContext db = new ApplicationContext())
            {
                constructions = db.Construction.ToList();
                Construction construction = await context.Request.ReadFromJsonAsync<Construction>();
                if (construction != null)
                {

                    //object1.ObjectsId = newId;

                    constructions.Add(construction);
                    await db.SaveChangesAsync();
                    await context.Response.WriteAsJsonAsync(construction);
                }
                else
                {
                    await context.Response.WriteAsJsonAsync("Constructor hasn't enought parameters");
                }
            }
    }

        //Update object
        public static async Task UpdateConstruction(HttpContext context)
        {
            Construction constructionData = await context.Request.ReadFromJsonAsync<Construction>();
            if (constructionData != null)
            {
                List<Construction> constructions;

                using (ApplicationContext db = new ApplicationContext())
                {
                    constructions = db.Construction.ToList();
                    var construction = constructions.FirstOrDefault(c => c.ConstructionId == constructionData.ConstructionId);
                    if (construction != null)
                    {
                        construction.Name = constructionData.Name;
                        construction.Description = constructionData.Description;
                        construction.IsWorkshop = constructionData.IsWorkshop;
                        await db.SaveChangesAsync();
                        await context.Response.WriteAsJsonAsync(construction);
                    }

                    else
                    {
                        await context.Response.WriteAsJsonAsync("Construction is null");
                    }
                }
            }
        }

    //Delete field 
    public static async void DeleteConstruction(HttpContext context, int id)
    {
        List<Construction> constructions;
            using (ApplicationContext db = new ApplicationContext())
            {
                constructions = db.Construction.ToList();
                Construction construction = constructions.FirstOrDefault((c) => c.ConstructionId == id);

                if (construction != null)
                {
                    constructions.Remove(construction);
                    await db.SaveChangesAsync();
                    await context.Response.WriteAsJsonAsync(construction);

                }
                // если не найден, отправляем статусный код и сообщение об ошибке
                else
                {
                    await context.Response.WriteAsJsonAsync("Construction doen't exist");
                }
            }
    }
}
}
