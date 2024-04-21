using System.Text.Json;
using System.Reflection;
using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using Backand.DbEntities.ConstructionSpace.JsonReaders;

namespace Backand.DbEntities.ConstructionSpace
{
   
    static class JsonSerializerOptionsExtension
    {
        public static JsonConverter<T> GetConverter<T>(this JsonSerializerOptions options)
        {
            return (JsonConverter<T>)options.GetConverter(typeof(T));
        }
    }
    public class ConstructionSerializer : JsonConverter<Construction>
    {
        private JsonConstructionPropertyReader[] readers = { new JsonInt32ConstructionReader(), new JsonStringConstructionReader() };
        private void ReadValueToData(ref Utf8JsonReader reader,JsonNamingPolicy? namingPolicy,Construction data)
        {
            string? prop = reader.GetString();
            if (prop != null)
            {
                string propertyName = namingPolicy?.ConvertName(prop) ?? prop;
                reader.Read();//читаю значение свойства
                foreach (var propReader in readers)
                {
                    bool readSuccess = propReader.ReadProperty(propertyName, ref reader, data);
                    if (readSuccess)
                        return;
                }
                throw new JsonException($"Неизвестное свойство '{propertyName}'!");
            }
            else
                throw new JsonException("Нечитаемое имя свойства!");
        }
        //Производится чтение только тогда, когда я добавляю сооружение в базу данных,
        //следовательно информацию о различных сложных типах можно передавать в виде id
        public override Construction? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Construction data = new() { ConstructionStateId=BuildState.Planned };
            using ApplicationContext context = new();
           // data.ConstructionState = context.ConstructionState.First(cs=>cs.ConstructionStateId==data.ConstructionStateId);
            
            var namingPolicy = options.PropertyNamingPolicy;
            if (namingPolicy is CustomCammelCase np)
                np.Read = true;
            
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject: continue;
                    case JsonTokenType.EndObject:
                        //data.ConstructionType = context.ConstructionType.First(ct=>ct.ConstructionTypeId==data.ConstructionTypeId);
                        return data;
                    case JsonTokenType.PropertyName: break;
                    default: throw new JsonException("Неправильный тип десериализуемого объекта!");
                }

                ReadValueToData(ref reader, namingPolicy, data);
                
            }
            
            throw new JsonException("Необрабатываемый объект!");
        }
        //Производится запись только для получения информации о сооружении со внешней стороны,
        //а значит нужна как можно более подробная информация о нём
        public override void Write(Utf8JsonWriter writer, Construction value, JsonSerializerOptions options)
        {

            Type constr_type = typeof(Construction);
            var namingPolicy =options.PropertyNamingPolicy;
            if (namingPolicy is CustomCammelCase np)
                np.Read = false;
            string[] ignorable = { /*nameof(value.ConstructionStateId), nameof(value.ConstructionTypeId), nameof(value.BuildWay)*/ };
            ignorable =ignorable.Select(namingPolicy.ConvertName).ToArray();
            writer.WriteStartObject();
            
            if (value != null)
            {
                PropertyInfo[] props = constr_type.GetProperties();
                foreach (var prop in props)
                {
                    string prop_name = namingPolicy.ConvertName(prop.Name);
                    bool has_jsonIgnore = prop.GetCustomAttribute<JsonIgnoreAttribute>() != null;
                    bool mustIgnore = ignorable.Contains(prop_name)||has_jsonIgnore;
                    if (!mustIgnore)
                    {
                        object? prop_value = prop.GetValue(value);
                        if (prop_value != null)
                        {
                            writer.WritePropertyName(prop_name);
                            Type prop_type = prop_value.GetType();
                            JsonSerializer.Serialize(writer, prop_value, prop_type,options);
                        }
                        else
                            writer.WriteNull(prop_name);
                    }
                }
            }
            else
                writer.WriteNullValue();
            writer.WriteEndObject();
        }
    }
}
