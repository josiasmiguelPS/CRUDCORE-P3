using NUnit.Framework;
using OpenQA.Selenium;
using CRUDCORE_P3.SeleniumTests.Helpers;
using AventStack.ExtentReports;

namespace CRUDCORE_P3.SeleniumTests.Tests
{
    public class BaseTest
    {
        protected IWebDriver driver;
        protected ExtentTest test;
        protected string baseUrl = "http://localhost:5259";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            ReportHelper.GetExtent();
        }

        [SetUp]
        public void Setup()
        {
            driver = DriverFactory.GetChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    string screenshotPath = ScreenshotHelper.TakeScreenshot(driver, TestContext.CurrentContext.Test.Name);
                    if (!string.IsNullOrEmpty(screenshotPath) && test != null)
                    {
                        test.AddScreenCaptureFromPath(screenshotPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en TearDown: {ex.Message}");
            }
            finally
            {
                // IMPORTANTE: Cerrar y limpiar el driver
                if (driver != null)
                {
                    try
                    {
                        driver.Quit();
                        driver.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error cerrando driver: {ex.Message}");
                    }
                }
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ReportHelper.FlushReport();
        }
    }
}