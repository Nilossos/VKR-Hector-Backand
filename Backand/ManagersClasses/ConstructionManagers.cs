using Backand.DbEntites;
using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using Microsoft.EntityFrameworkCore;

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
        public static async Task<IResult> GetPlannedConstructions(ApplicationContext dbContext)
        {
            return await Task.Run(() =>
            {
                var constructions = dbContext.Construction.Where(c => c.ConstructionStateId == BuildState.Planned);
                var data = constructions.Join(dbContext.Objects,
                    c => c.ObjectsId,
                    o => o.ObjectsId,
                    (c, o) => new {
                        Construction = new EntityLink() { Id = c.ConstructionId, Name = c.ConstructionName },
                        Object = new EntityLink() { Id = o.ObjectsId, Name = o.Name },
                        Mine=new EntityLink()
                        {
                            Id=o.MineId,
                            Name = dbContext.Mine.First(m => m.MineId == o.MineId).Name
                        }
                        
                        
                    }
                ) ;
                return Results.Json(data);
            });
        }
        public static async Task<IResult> GetConstructionById(int construction_id, ApplicationContext dbContext,HttpContext httpContext)
        {
            var task = Task.Run(() =>
            {
                Construction construction = dbContext.Construction.FirstOrDefault(c => c.ConstructionId == construction_id);
                if (construction != null)
                {
                    ConstructionInfo info = new(construction, dbContext);
                    return Results.Json(info);
                }
                else
                {
                    httpContext.Response.StatusCode = 404;
                    return Results.Json(new BaseResponse(true,$"Сооружение с id {construction_id} не найдено!"));
                }
               
                
            });
            return await task;
        }
        //Create new object 
        public static async Task CreateConstruction(HttpContext context,ApplicationContext dbContext)
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
                response =new(true,exc.ToString());
            }
            await context.Response.WriteAsJsonAsync(response);
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
        public static async Task<IResult> DeleteConstruction(ApplicationContext dbContext,HttpContext context, int id)
        {
            BaseResponse response;
            var deletable = await dbContext.Construction.FirstOrDefaultAsync(c => c.ConstructionId == id);
            if (deletable != null)
            {
                try
                {
                    dbContext.Construction.Remove(deletable);
                    await dbContext.SaveChangesAsync();
                    response = new(false, $"Удание сооружения {id} прошло успешно!");
                }
                catch (Exception exc)
                {
                    context.Response.StatusCode = 500;
                    response = new(true, exc.ToString());
                }
            }
            else
            {
                context.Response.StatusCode = 404;
                response = new(true, $"Не найдено сооружения с id {id}!");
            }
            return Results.Json(response);
        }
    }
}
