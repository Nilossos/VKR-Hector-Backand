using Backand.Services.WebDriverServiceSpace;
using OpenQA.Selenium;
using static Newtonsoft.Json.JsonConvert;

namespace Backand.Services
{
    internal class DistanceService
    {
        IWebElement input;
        IWebElement output;
        IWebElement reset_btn;
        public DistanceService(WebDriver driver)
        {
            input = driver.FindElement(By.Id("input"));
            output = driver.FindElement(By.Id("output"));
            reset_btn = driver.FindElement(By.Id("reset"));
        }
        public DistanceService(IServiceProvider provider) : this(provider.GetRequiredService<WebDriverService>().Value)
        {

        }
        
        public async Task<double> GetDistance(params double[][] routes)
        {
            input.SendKeys(SerializeObject(routes));
            output.Click();
            double distance = await Task.Run(() =>
            {
                while (output.GetAttribute("read") == "0") { }
                string dist_str = output.GetAttribute("value");
                double distance = DeserializeObject<double>(dist_str);
                if (distance > -1)
                    return distance;
                else
                {
                    string error=output.GetAttribute("error");
                    throw new Exception(error);
                }
            });
            reset_btn.Click();
            return distance;
        }
    }
    public static class DistanceServiceExtension
    {
        public static void AddDistanceService(this IServiceCollection services)
        {
            services.AddSingleton(x => new DistanceService(x));
        }
    }
}
