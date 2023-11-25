using Backand.FrontendEntities.Links;

namespace Backand.FrontendEntities
{
    public record ConstructionTable(
        EntityLink Construction, 
        EntityLink Object, 
        EntityLink Mine, 
        EntityLink Subsidiary);
}
