namespace Backand.ManagersClasses
{
    public class SubsidiaryManager
    {
        public static async Task<IResult> GetSubsidiaries(ApplicationContext dbContext)
        {
            return await Task.Run(() =>
            {
                var subs = dbContext.Subsidiary.ToArray();
                return Results.Json(subs);
            });
        }
    }
}
