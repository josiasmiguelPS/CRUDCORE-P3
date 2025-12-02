using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using AventStack.ExtentReports;
using CRUDCORE_P3.SeleniumTests.Helpers;
using System;
using System.IO;

namespace CRUDCORE_P3.SeleniumTests
{
    public class BaseTest
    {
        protected IWebDriver driver;
        protected static ExtentReports extent;
        protected ExtentTest test;
        protected string baseUrl = "http://localhost:5259";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            extent = ReportHelper.GetExtent();
            Console.WriteLine("Configuración inicial de ExtentReports completada");
        }

        [SetUp]
        public virtual void Setup() // IMPORTANTE: virtual permite override
        {
            // Configurar ChromeDriver
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-popup-blocking");

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Cookies.DeleteAllCookies();
        }

        [TearDown]
        public virtual void TearDown() // IMPORTANTE: virtual permite override
        {
            try
            {
                // Capturar screenshot si el test falló
                if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    string screenshotPath = CaptureScreenshot($"FAILED_{TestContext.CurrentContext.Test.Name}");
                    test?.Fail("Test falló - Ver screenshot",
                        MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al capturar screenshot: {ex.Message}");
            }
            finally
            {
                driver?.Quit();
                driver?.Dispose();
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ReportHelper.FlushReport();
        }

        protected string CaptureScreenshot(string screenshotName)
        {
            string screenshotFolder = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
            if (!Directory.Exists(screenshotFolder))
            {
                Directory.CreateDirectory(screenshotFolder);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"{screenshotName}_{timestamp}.png";
            string fullPath = Path.Combine(screenshotFolder, fileName);

            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(fullPath);
                Console.WriteLine($"Screenshot guardado: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar screenshot: {ex.Message}");
            }

            return fullPath;
        }

        protected void AddScreenshotToReport(string screenshotName)
        {
            try
            {
                string path = CaptureScreenshot(screenshotName);
                test?.Info("Screenshot capturado",
                    MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar screenshot al reporte: {ex.Message}");
            }
        }

        protected void LogInfo(string message)
        {
            test?.Log(Status.Info, message);
            Console.WriteLine($"[INFO] {message}");
        }

        protected void LogPass(string message)
        {
            test?.Log(Status.Pass, message);
            Console.WriteLine($"[PASS] {message}");
        }

        protected void LogFail(string message)
        {
            test?.Log(Status.Fail, message);
            Console.WriteLine($"[FAIL] {message}");
        }

        protected void LogWarning(string message)
        {
            test?.Log(Status.Warning, message);
            Console.WriteLine($"[WARN] {message}");
        }
    }
}