namespace Backand.FrontendEntities.Links
{
    public class ObjectWithTransportTypesLink
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public int?[] TransportTypes {  get; init; }
    }
}
