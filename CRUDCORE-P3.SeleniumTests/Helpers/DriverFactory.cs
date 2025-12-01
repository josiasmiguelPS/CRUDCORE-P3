using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CRUDCORE_P3.SeleniumTests.Helpers
{
    public class DriverFactory
    {
        public static IWebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");

            return new ChromeDriver(options);
        }
    }
}