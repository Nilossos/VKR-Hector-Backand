﻿using Backand.DbEntities;
using Backand.DbEntities.ConstructionSpace;
using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Backand.ManagersClasses
{

    public static class ConstructionManagers
    {
        //Get all construction
        public static async Task GetAllConstructions(HttpContext context)
        {

            List<Construction> constructions;
            using (ApplicationContext db = new ApplicationContext())
            {
                constructions = db.Construction.ToList();
            }

            await context.Response.WriteAsJsonAsync(constructions);
        }
        //Get by object id
        public static async Task<IResult> GetConstructionsByObjectId(int object_id, ApplicationContext dbContext)
        {
            var constructions = await dbContext.Construction
                .Include(c => c.ConstructionState)
                .Include(c => c.BuildWay)
                .Where(c => c.ObjectId == object_id)
                .ToListAsync();
            return Results.Json(constructions);
        }
        //Был заменен на GetObjectWithTransportTypes
        private static async Task<(EntityLink, ObjectEntity)> GetObjectWithTransportTypes(ApplicationContext dbContext, Construction c)
        {
            await dbContext.Entry(c).Reference(c => c.Object).LoadAsync();
            DbEntities.ObjectEntity bash = c.Object;
            EntityLink bLink = new() { Id = bash.ObjectId, Name = bash.Name };
            return (bLink, bash);
        }
        private static async Task<(ObjectWithTransportTypesLink, ObjectEntity)> GetObjectWithTransportTypesLink(ApplicationContext dbContext, Construction c)
        {
            await dbContext.Entry(c)
                   .Reference(c => c.Object)
                   .Query()
                   .Include(o => o.ObjectTransportTypes) // Загружаем ObjectTransportTypes
                   .LoadAsync();
            DbEntities.ObjectEntity bash = c.Object;
            int[] transportTypeIds = bash.ObjectTransportTypes
                             .Select(ott => ott.TransportTypeId)
                             .Distinct()
                             .ToArray();
            ObjectWithTransportTypesLink bLink = new() { Id = bash.ObjectId, Name = bash.Name, TransportTypes = transportTypeIds };
            return (bLink, bash);
        }
        private static async Task<(EntityLink, Mine)> GetMineLink(ApplicationContext dbContext, DbEntities.ObjectEntity bash)
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
            var constructions = await dbContext.Construction
             .Where(c => c.ConstructionStateId == BuildState.Planned)
             .Select(c => new
             {
                 // ... other Construction properties (map them correctly)
                 ConstructionId = c.ConstructionId, // Example
                 ConstructionName = c.ConstructionName, // Example

                 Object = new
                 {
                     ObjectId = c.Object.ObjectId,
                     Name = c.Object.Name,
                     Mine = new
                     {
                         MineId = c.Object.Mine.MineId,
                         Name = c.Object.Mine.Name,
                         Subsidiary = new
                         {
                             SubsidiaryId = c.Object.Mine.Subsidiary.SubsidiaryId,
                             Name = c.Object.Mine.Subsidiary.Name,
                         }
                     },
                     TransportTypes = c.Object.ObjectTransportTypes.Select(t => new
                     {
                         TransportTypeId = t.TransportType.TransportTypeId,
                         TransportTypeName = t.TransportType.Name,
                     }).ToList()
                 }
             })
             .ToListAsync();

            //List<ConstructionTable> tables = new();
            //foreach (var c in constructions)
            //{
            //    EntityLink cLink = new() { Id = c.ConstructionId, Name = c.ConstructionName };

            //    var (bLink, bash) = await GetObjectWithTransportTypesLink(dbContext, c);
            //    var (mLink, mine) = await GetMineLink(dbContext, bash);
            //    var sLink = await GetSubsidiaryLink(dbContext, mine);

            //    ConstructionTable table = new(cLink, bLink, mLink, sLink);
            //    tables.Add(table);
            //}

            return Results.Json(constructions);
        }
        public static async Task<IResult> GetConstructionById(int construction_id, ApplicationContext dbContext, HttpContext httpContext)
        {
            Construction construction = await dbContext.Construction.FirstOrDefaultAsync(c => c.ConstructionId == construction_id);
            if (construction != null)
            {
                var cEntry = dbContext.Entry(construction);
                await cEntry.Reference(c => c.ConstructionState).LoadAsync();
                await cEntry.Reference(c => c.ConstructionType).LoadAsync();

                return Results.Json(construction, new JsonSerializerOptions() { PropertyNamingPolicy = new CustomCammelCase(), WriteIndented = true });
            }
            else
            {
                httpContext.Response.StatusCode = 404;
                return Results.Json(new BaseResponse(true, $"Сооружение с id {construction_id} не найдено!"));
            }

        }
        //Create new object 
        public static async Task CreateConstruction(HttpContext context, ApplicationContext dbContext)
        {
            try
            {
                var constructions = dbContext.Construction;

                Construction? construction = await context.Request.ReadFromJsonAsync<Construction>(new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = new CustomCammelCase()
                });

                if (construction.ConstructionName == "")
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new BaseResponse(true, "Название обязательно для заполнения."));
                    return;
                }

                if (construction.ConstructionTypeId == 0)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new BaseResponse(true, "Тип обязателен для заполнения."));
                    return;
                }

                if (construction != null)
                {
                    construction.ConstructionStateId = BuildState.Planned;
                    await constructions.AddAsync(construction);
                    await dbContext.SaveChangesAsync();

                    await context.Response.WriteAsJsonAsync(new BaseResponse(false, "Сооружение добавлено!"));
                    return; // Important: Exit early after successful processing
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest; // Explicitly set 400
                    await context.Response.WriteAsJsonAsync(new BaseResponse(true, "Неправильные передаваемые данные о сооружении!"));
                    return; // Exit
                }
            }
            catch (Exception exc)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError; // Set 500 for server errors
                await context.Response.WriteAsJsonAsync(new BaseResponse(true, exc.Message)); // Log just the message, not the whole stack trace for security
                return; // Exit
            }
        }

        //Update object
        public static async Task<IResult> UpdateConstruction(int construction_id_update, HttpContext context, ApplicationContext dbContext)
        {
            //var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            //Console.WriteLine("Received JSON:");
            //Console.WriteLine(requestBody); // Логируем присланный JSON

            Construction? constructionData = await context.Request.ReadFromJsonAsync<Construction>(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = new CustomCammelCase()
            });
            var a = 1;
            if (constructionData != null)
            {
                var construction = await dbContext.Construction
                    .Include(c => c.ConstructionState)
                    .SingleOrDefaultAsync(c => c.ConstructionId == construction_id_update);
                //var construction = await dbContext.Construction.Include(construction => construction.ConstructionState).FindAsync(construction_id_update);
                if (construction != null)
                {
                    if (construction.ConstructionState.ConstructionStateId == BuildState.Planned)
                    {
                        // Обновляем поля
                        construction.ConstructionName = constructionData.ConstructionName;
                        construction.ConstructionType = constructionData.ConstructionType;

                        await dbContext.SaveChangesAsync();

                        return Results.Json(construction); // Возвращаем обновлённое сооружение
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        return Results.Json(new { message = "Нельзя обновить существующее сооружение" });
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                    return Results.Json(new { message = "Сооружение не найдено" });
                }
            }

            context.Response.StatusCode = 400;
            return Results.Json(new { message = "Что-то не так с данными сооружения" });
        }

        //Delete field 
        public static async Task<IResult> DeleteConstruction(int construction_id_delete, ApplicationContext dbContext, HttpContext context)
        {
            BaseResponse response;
            var deletable = await dbContext.Construction.FirstOrDefaultAsync(c => c.ConstructionId == construction_id_delete);
            if (deletable != null)
            {
                if (deletable.ConstructionStateId == BuildState.Planned)
                {
                    try
                    {
                        dbContext.Construction.Remove(deletable);
                        await dbContext.SaveChangesAsync();
                        response = new(false, $"Удание сооружения {construction_id_delete} прошло успешно!");
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
                    response = new(true, $"Нельзя удалить существующее сооружение!");
                }
            }
            else
            {
                context.Response.StatusCode = 404;
                response = new(true, $"Не найдено сооружения с id {construction_id_delete}!");
            }
            return Results.Json(response);
        }
    }
}
