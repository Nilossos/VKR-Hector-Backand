using Backand.DbEntities;
using Backand.DbEntities.ConstructionSpace;

namespace Backand.FrontendEntities
{
    public class ConstructionInfo
	{
		public int ConstructionId { get; }
		public string ConstructionName { get; }
		public int ObjectsId { get; }
		public ConstructionType ConstructionType { get; }
		public ConstructionState BuildState { get; }
		public ConstructionInfo(Construction construction, ApplicationContext dbContext)
		{
			ConstructionId = construction.ConstructionId;
			ConstructionName = construction.ConstructionName;
			ObjectsId = construction.ObjectsId;


			ConstructionType type = dbContext.ConstructionType.First(ct => ct.ConstructionTypeId == construction.ConstructionTypeId);
			ConstructionState state = dbContext.ConstructionState.First(cs => cs.ConstructionStateId == construction.ConstructionStateId);
			BuildState = state;
			ConstructionType = type;
		}

	}
}
