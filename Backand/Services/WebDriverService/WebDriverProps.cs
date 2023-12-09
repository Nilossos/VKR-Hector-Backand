namespace Backand.Services.WebDriverServiceSpace
{
    public readonly struct WebDriverProps
    {
        public string DriverPath { get; }
        public WebDriverType DriverType { get; }
        public string HtmlPath { get; }
        public string YamapsScriptLink { get; }
        public WebDriverProps(IConfiguration config)
        {
            IConfiguration custom = config.GetSection("Custom");
            DriverPath = custom["Engine:driverPath"]??"";
            HtmlPath = custom["distanceHtml"]??"";
            DriverType = WDTypesTable.Table[custom["Engine:type"]];
            YamapsScriptLink = GetScriptLink(config);
        }
        private string GetScriptLink(IConfiguration config)
        {
            IConfiguration yamaps = config.GetSection("yamaps");
            string protocol = yamaps["protocol"];
            string domain = yamaps["domain"];
            string version = yamaps["version"];
            string apikey = yamaps["apikey"];
            string src = $"{protocol}://{domain}/{version}/?apikey={apikey}&lang=en_RU";
            return src;
        }
    }
}
