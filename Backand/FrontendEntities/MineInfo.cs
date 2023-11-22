using Backand.DbEntities;
using Backand.FrontendEntities.Links;

namespace Backand.FrontendEntities
{
    public class MineInfo:Mine
    {
        public Subsidiary Subsidiary { get; }
        public MineInfo(ApplicationContext dbContext,Mine mine)
        {
            MineId =mine.MineId;
            Name =mine.Name;
            Coordinates = mine.Coordinates;
            var subs =dbContext.Subsidiary.ToArray();
            Subsidiary company=dbContext.Subsidiary.First(s=>s.SubsidiaryId==mine.SubsidiaryId);
            Subsidiary =company;
        }
    }
}
