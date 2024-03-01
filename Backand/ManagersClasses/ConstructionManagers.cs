using Backand.DbEntities;
using Backand.DbEntities.ConstructionSpace;
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
        private static async Task<(EntityLink,Objects)> GetObjectsLink(ApplicationContext dbContext,Construction c)
        {
            await dbContext.Entry(c).Reference(c => c.Object).LoadAsync();
            Objects bash = c.Object;
            EntityLink bLink = new() { Id = bash.ObjectsId, Name = bash.Name };
            return (bLink, bash);
        }
        private static async Task<(EntityLink,Mine)> GetMineLink(ApplicationContext dbContext,Objects bash)
        {
            await dbContext.Entry(bash).Reference(b => b.Mine).LoadAsync();
            Mine mine = bash.Mine;
            EntityLink mLink = new() { Id = mine.MineId, Name = mine.Name };
            return (mLink, mine);
        }
        private static async Task<EntityLink> GetSubsidiaryLink(ApplicationContext dbContext, Mine mine)
        {
            await dbContext.Entry(mine).Reference(m => m.Subsidiary).LoadAsync();
            Subsidiary subs = mine.Subsidiary;
            EntityLink sLink = new() { Id = subs.SubsidiaryId, Name = subs.Name };
            return sLink;
        }
        public static async Task<IResult> GetPlannedConstructions(ApplicationContext dbContext)
        {
            var constructions = await dbContext.Construction.
                                        Where(c => c.ConstructionStateId == BuildState.Planned).
                                        ToListAsync();
            List<ConstructionTable> tables = new();
            foreach (var c in constructions)
            {
                EntityLink cLink = new() {Id=c.ConstructionId, Name=c.ConstructionName };

                var (bLink, bash) = await GetObjectsLink(dbContext,c);
                var (mLink, mine) = await GetMineLink(dbContext, bash);
                var sLink = await GetSubsidiaryLink(dbContext,mine);
                
                ConstructionTable table = new(cLink,bLink,mLink,sLink);
                tables.Add(table);
            }
            
            return Results.Json(tables);
        }
        public static async Task<IResult> GetConstructionById(int construction_id, ApplicationContext dbContext,HttpContext httpContext)
        {
            Construction construction =await dbContext.Construction.FirstOrDefaultAsync(c => c.ConstructionId == construction_id);
            if (construction != null)
            {
                var cEntry = dbContext.Entry(construction);
                await cEntry.Reference(c => c.ConstructionState).LoadAsync();
                await cEntry.Reference(c => c.ConstructionType).LoadAsync();
                return Results.Json(construction);
            }
            else
            {
                httpContext.Response.StatusCode = 404;
                return Results.Json(new BaseResponse(true, $"Сооружение с id {construction_id} не найдено!"));
            }
            
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
                    ConstructionState? plannedState = await dbContext.ConstructionState.FirstOrDefaultAsync(cs=>
                                                        cs.ConstructionStateId==BuildState.Planned);
                    if (plannedState != null) {
                        construction.ConstructionState = plannedState;
                        construction.ConstructionId = 0;
                        await constructions.AddAsync(construction);
                        await dbContext.SaveChangesAsync();
                        response = new(false, "Сооружение добавлено!");
                    }
                    else
                        response = new(true, "Состояния строительства 'Запланированно' не существует");
                }
                else
                    response = new(true, "Неправильные передаваемые данные о сооружении!");

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
