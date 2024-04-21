using System.Text.Json;

namespace Backand.DbEntities.ConstructionSpace.JsonReaders
{
    class JsonStringConstructionReader : JsonConstructionPropertyReader
    {
        private string[] props = { nameof(Construction.ConstructionName) };
        public override string[] AvailableProps => props;

        protected override object GetValue(ref Utf8JsonReader reader) => 
            reader.GetString();
    }
}
