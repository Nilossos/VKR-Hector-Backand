namespace Backand.ManagersClasses
{
    public static class ConstructionTypeManager
    {
        public static async Task<IResult> GetTypes(ApplicationContext dbContext)
        {
            return await Task.Run(() =>
            {
                var list = dbContext.ConstructionType.ToArray();
                return Results.Json(list);
            });
        }
    }
}
