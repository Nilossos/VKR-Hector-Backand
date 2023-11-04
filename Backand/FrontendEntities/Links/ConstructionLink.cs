using System.Text.Json.Serialization;

namespace Backand.FrontendEntities.Links
{
    public readonly struct ConstructionLink
    {
        [JsonPropertyName("object_id")]
        public int ObjectId { get; }
        [JsonPropertyName("local_id")]
        public int ConstructionId { get; }
        [JsonPropertyName("name")]
        public string Name { get; }
        public ConstructionLink(int object_id, int construction_id, string name)
        {
            ObjectId = object_id;
            ConstructionId = construction_id;
            Name = name;
        }
    }
}
