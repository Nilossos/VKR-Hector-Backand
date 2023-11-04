using Backand.DbEntites;
using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;

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
        //Get by object id
        public static async Task<IResult> GetConstructionsByObjectId(int object_id, ApplicationContext dbContext)
        {
            EntityLink[] links = await Task.Run(() =>
            {
                var _links = dbContext.Construction.
                    Where(c=>c.ObjectsId==object_id).
                    Select(c=>c.Link).ToArray();
                return _links;
            });
            return Results.Json(links);
        }
        public static async Task<IResult> GetConstructionById(int construction_id, ApplicationContext dbContext)
        {
            var task = Task.Run(() =>
            {
                Construction construction = dbContext.Construction.First(c => c.ConstructionId == construction_id);
                ConstructionInfo info = new(construction, dbContext);
                return Results.Json(info);
            });
            return await task;
        }
        //Create new object 
        public static async Task<IResult> CreateConstruction(HttpContext context,ApplicationContext dbContext)
        {
            BaseResponse response;
            try
            {
                var constructions = dbContext.Construction;

                Construction? construction = await context.Request.ReadFromJsonAsync<Construction>();

                if (construction != null)
                {
                    construction.ConstructionStateId =BuildState.Planned;
                    construction.ConstructionId = 0;
                    await constructions.AddAsync(construction);
                    await dbContext.SaveChangesAsync();
                    response = new(false, "Construction has added!");
                }
                else
                    response = new(true, "Incorrect construction data!");

            }
            catch(Exception exc)
            {
                response =new(false,exc.ToString());
            }
            return Results.Json(response);
        }

            //Update object
            public static async Task UpdateConstruction(HttpContext context)
            {
                /*Construction constructionData = await context.Request.ReadFromJsonAsync<Construction>();
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
                }*/
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
