namespace Backand.Services.WebDriverServiceSpace
{
    public enum WebDriverType
    {
        Chrome,
        Firefox,
        IE
    }
    public static class WDTypesTable
    {
        public static readonly Dictionary<string, WebDriverType> Table = new()
        {
            { "chrome", WebDriverType.Chrome},
            { "firefox", WebDriverType.Firefox },
            { "ie", WebDriverType.IE }
        };
    }
}
