using Backand;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses
{
    public static class ConstructionStateManager
    {
        public static async Task<IResult> GetAvailableStates(ApplicationContext db)
        {
            var data=await db.ConstructionState.ToListAsync();
            return Results.Json(data);
        }
    }
}
