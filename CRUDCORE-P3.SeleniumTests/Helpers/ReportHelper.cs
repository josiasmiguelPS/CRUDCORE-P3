using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace CRUDCORE_P3.SeleniumTests.Helpers
{
    public class ReportHelper
    {
        private static ExtentReports extent;
        private static readonly string reportPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Reports",
            $"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.html"
        );

        public static ExtentReports GetExtent()
        {
            if (extent == null)
            {
                var reportDir = Path.GetDirectoryName(reportPath);
                if (!Directory.Exists(reportDir))
                {
                    Directory.CreateDirectory(reportDir);
                }

                var htmlReporter = new ExtentSparkReporter(reportPath);
                htmlReporter.Config.DocumentTitle = "Reporte de Pruebas - CRUDCORE-P3";
                htmlReporter.Config.ReportName = "Pruebas Automatizadas con Selenium";
                htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Dark;

                extent = new ExtentReports();
                extent.AttachReporter(htmlReporter);
                extent.AddSystemInfo("Aplicación", "CRUDCORE-P3");
                extent.AddSystemInfo("Ambiente", "Testing");
                extent.AddSystemInfo("Usuario", Environment.UserName);
            }
            return extent;
        }

        public static void FlushReport()
        {
            extent?.Flush();
            Console.WriteLine($"Reporte HTML generado: {reportPath}");
        }
    }
}