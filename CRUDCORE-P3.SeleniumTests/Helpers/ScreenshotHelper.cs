using OpenQA.Selenium;

namespace CRUDCORE_P3.SeleniumTests.Helpers
{
    public class ScreenshotHelper
    {
        private static readonly string screenshotPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Screenshots"
        );

        static ScreenshotHelper()
        {
            if (!Directory.Exists(screenshotPath))
            {
                Directory.CreateDirectory(screenshotPath);
            }
        }

        public static string TakeScreenshot(IWebDriver driver, string testName)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileName = $"{testName}_{timestamp}.png";
                string fullPath = Path.Combine(screenshotPath, fileName);

                screenshot.SaveAsFile(fullPath);
                Console.WriteLine($"Screenshot guardado: {fullPath}");

                return fullPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al tomar screenshot: {ex.Message}");
                return string.Empty;
            }
        }
    }
}