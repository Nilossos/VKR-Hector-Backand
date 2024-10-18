using Backand.FrontendEntities.Links;

namespace Backand.FrontendEntities
{
    public record ConstructionTable(
        EntityLink Construction,
        ObjectWithTransportTypesLink Object, 
        EntityLink Mine, 
        EntityLink Subsidiary);
}
