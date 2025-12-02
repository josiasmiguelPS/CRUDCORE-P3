using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using AventStack.ExtentReports.Reporter.Configuration;

namespace CRUDCORE_P3.SeleniumTests.Utilities
{
    public class ReportManager
    {
        private static ExtentReports extent;
        private static string reportPath;

        public static ExtentReports GetExtentReports()
        {
            if (extent == null)
            {
                // Crear carpeta de reportes si no existe
                string reportFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                Directory.CreateDirectory(reportFolder);

                // Nombre del archivo con timestamp
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                reportPath = Path.Combine(reportFolder, $"TestReport_{timestamp}.html");

                // Configurar el reporte HTML
                var htmlReporter = new ExtentHtmlReporter(reportPath);

                // CONFIGURACIÓN VISUAL DEL REPORTE
                htmlReporter.Config.DocumentTitle = "Reporte de Pruebas - CRUDCORE-P3";
                htmlReporter.Config.ReportName = "Pruebas Automatizadas con Selenium";
                htmlReporter.Config.Theme = Theme.Dark; // Tema oscuro (puedes cambiar a Theme.Standard)
                htmlReporter.Config.Encoding = "UTF-8";
                htmlReporter.Config.CSS = @"
                    .test-name { font-size: 16px; font-weight: bold; }
                    .category-name { font-size: 14px; color: #00bcd4; }
                    .node-name { font-size: 15px; }
                ";

                // Crear instancia de ExtentReports
                extent = new ExtentReports();
                extent.AttachReporter(htmlReporter);

                // INFORMACIÓN DEL SISTEMA
                extent.AddSystemInfo("Aplicación", "CRUDCORE-P3");
                extent.AddSystemInfo("Entorno", "Testing");
                extent.AddSystemInfo("Usuario", Environment.UserName);
                extent.AddSystemInfo("SO", Environment.OSVersion.ToString());
                extent.AddSystemInfo("Framework", ".NET 8.0");
                extent.AddSystemInfo("Navegador", "Chrome");
                extent.AddSystemInfo("Selenium", "4.x");

                Console.WriteLine($"Reporte configurado en: {reportPath}");
            }

            return extent;
        }

        public static void FlushReport()
        {
            extent?.Flush();
            Console.WriteLine($"Reporte generado exitosamente en: {reportPath}");
            Console.WriteLine($"Abre el reporte en tu navegador: file:///{reportPath.Replace("\\", "/")}");
        }

        public static string GetReportPath()
        {
            return reportPath;
        }
    }
}