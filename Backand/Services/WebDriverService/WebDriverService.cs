using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Specialized;

namespace Backand.Services.WebDriverServiceSpace
{
    public class WebDriverService
    {
        public WebDriver Value { get; }
        public WebDriverService(WebDriverProps props)
        {
           
            string html_path = Directory.GetCurrentDirectory() + "\\Assets\\" + props.HtmlPath.Replace('/', '\\');
            html_path = $"file:///{html_path.Replace("\\", "//")}";
            ChromeOptions options = new();
            options.AddArgument("--headless");
            ChromeDriver chrome = new(props.DriverPath, options) { Url = html_path };
            IWebElement scriptElem = chrome.FindElement(By.Id("yamaps-root-script"));
            string src = props.YamapsScriptLink;
            chrome.ExecuteScript($"arguments[0].setAttribute('src','{src}')", scriptElem);
            IWebElement init = chrome.FindElement(By.Id("init"));
            init.Click();
            Value = chrome;
            WaitIniting(init);
        }
        private void WaitIniting(IWebElement init_button)
        {
            double start = DateTime.Now.TimeOfDay.TotalSeconds;
            
            const double max_seconds = 30;
            while (init_button.GetAttribute("inited") == "0")
            {
                double now = DateTime.Now.TimeOfDay.TotalSeconds;
                if (now - start > max_seconds)
                    throw new TimeoutException("Превышено время ожидания получения дистанции");
            }
        }
    }
    public static class WebDriverServiceExtension
    {
        public static void AddWebDriverService(this IServiceCollection services)
        {
            
            services.AddSingleton(x =>
            {
                var conf = x.GetRequiredService<IConfiguration>();
                WebDriverProps props = new(conf);
                return new WebDriverService(props);
            });
        }
    }

}
