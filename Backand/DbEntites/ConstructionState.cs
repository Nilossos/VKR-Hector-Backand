namespace Backand.DbEntites
{
    public enum BuildState
    {
        Planned=1,
        Verified,
        Building,
        Built
    }
    public class ConstructionState
    {
        public BuildState ConstructionStateId { get; set; }
        public string Name { get; set; }
    }
}
