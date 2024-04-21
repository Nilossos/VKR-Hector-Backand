using System.Text.Json;

namespace Backand.DbEntities.ConstructionSpace;
//пришлось сделать свой cammelCase, так как обычный не работает
class CustomCammelCase : JsonNamingPolicy
{
    public bool Read { get; set; } = false;
    private char GetFirstSymbol(string propertyName)
    {
        char symbol = propertyName[0];
        Func<char, char> converter = Read ? char.ToUpper : char.ToLower;
        return converter(symbol);
    }
    public override string ConvertName(string name)
    {
        char firstSymbol = GetFirstSymbol(name);
        return firstSymbol + name.Remove(0, 1);
    }
}
