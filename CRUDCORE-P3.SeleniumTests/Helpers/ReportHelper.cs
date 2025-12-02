using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using AventStack.ExtentReports.Reporter.Configuration;
using System;
using System.IO;

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

                // ExtentSparkReporter es la versión 5.x
                var sparkReporter = new ExtentSparkReporter(reportPath);

                // Configuración del reporte
                sparkReporter.Config.DocumentTitle = "Reporte de Pruebas - CRUDCORE-P3";
                sparkReporter.Config.ReportName = "Pruebas Automatizadas con Selenium";
                sparkReporter.Config.Theme = Theme.Dark;
                sparkReporter.Config.Encoding = "UTF-8";
                sparkReporter.Config.TimeStampFormat = "MMM dd, yyyy HH:mm:ss";

                extent = new ExtentReports();
                extent.AttachReporter(sparkReporter);

                // Información del sistema
                extent.AddSystemInfo("Aplicación", "CRUDCORE-P3");
                extent.AddSystemInfo("Ambiente", "Testing");
                extent.AddSystemInfo("Usuario", Environment.UserName);
                extent.AddSystemInfo("Sistema Operativo", Environment.OSVersion.ToString());
                extent.AddSystemInfo(".NET Version", Environment.Version.ToString());
                extent.AddSystemInfo("Navegador", "Chrome");
            }
            return extent;
        }

        public static void FlushReport()
        {
            extent?.Flush();
            Console.WriteLine($"✅ Reporte HTML generado exitosamente en: {reportPath}");
        }
    }
}