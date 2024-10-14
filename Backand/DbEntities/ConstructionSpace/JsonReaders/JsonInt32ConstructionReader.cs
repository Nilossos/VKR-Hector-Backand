using System.Text.Json;

namespace Backand.DbEntities.ConstructionSpace.JsonReaders
{
    
    class JsonInt32ConstructionReader : JsonConstructionPropertyReader
    {
        private string[] props = { nameof(Construction.ConstructionTypeId), nameof(Construction.ConstructionId), nameof(Construction.ObjectId) };
        public override string[] AvailableProps => props;
        protected override object GetValue(ref Utf8JsonReader reader)=>
            reader.GetInt32();
        
    }
}
