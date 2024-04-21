using System.Text.Json;

namespace Backand.DbEntities.ConstructionSpace.JsonReaders;
abstract class JsonConstructionPropertyReader
{
    public abstract string[] AvailableProps { get; }
    private readonly Type typeToConvert=typeof(Construction);
    protected abstract object GetValue(ref Utf8JsonReader reader);
    public bool ReadProperty(string property, ref Utf8JsonReader reader, Construction output)
    {
        if (AvailableProps.Contains(property))
        {
            object value = GetValue(ref reader);
            typeToConvert.GetProperty(property).SetValue(output, value);
            return true;
        }
        return false;
    }
}

