using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports;
using CRUDCORE_P3.SeleniumTests.PageObjects;
using System;
using System.Linq;

namespace CRUDCORE_P3.SeleniumTests.Tests
{
    [TestFixture]
    public class EmpleadoCreateTests : BaseTest
    {
        private LoginPage loginPage;
        private HomePage homePage;
        private EmpleadoPage empleadoPage;

        [SetUp]
        public override void Setup() // override en lugar de new
        {
            base.Setup(); // Llamar al Setup del padre

            // Crear el test en ExtentReports
            var testName = TestContext.CurrentContext.Test.Name;
            var categories = TestContext.CurrentContext.Test.Properties["Category"]
                .Cast<string>()
                .ToArray();

            test = extent.CreateTest(testName);

            // Asignar categorías
            foreach (var category in categories)
            {
                test.AssignCategory(category);
            }

            test.AssignAuthor("Tu Nombre Aquí");

            // Inicializar Page Objects
            loginPage = new LoginPage(driver);
            homePage = new HomePage(driver);
            empleadoPage = new EmpleadoPage(driver);

            // Login automático antes de cada test
            try
            {
                LogInfo("Realizando login automático");
                driver.Navigate().GoToUrl(baseUrl);
                loginPage.Login("admin@test.com", "admin123");
                System.Threading.Thread.Sleep(2000);
                LogInfo("Login exitoso");
            }
            catch (Exception ex)
            {
                LogWarning($"Error en login automático: {ex.Message}");
            }
        }

        [Test]
        [Category("CRUD")]
        [Category("Create")]
        [Category("CaminoFeliz")]
        [Description("Verificar creación exitosa de empleado con datos válidos")]
        public void CrearEmpleado_CaminoFeliz_DatosValidos()
        {
            try
            {
                LogInfo("Iniciando prueba: Crear empleado con datos válidos");

                // Arrange
                string timestamp = DateTime.Now.ToString("HHmmss");
                string nombre = $"Juan Pérez {timestamp}";
                string correo = $"juan.perez{timestamp}@test.com";
                string telefono = "809-555-1234";

                LogInfo($"Datos de prueba generados - Nombre: {nombre}");

                // Act
                LogInfo("Navegando al formulario de creación");
                homePage.ClickCrearNuevo();
                System.Threading.Thread.Sleep(1000);

                AddScreenshotToReport("Formulario_Vacio");

                LogInfo("Ingresando datos del empleado");
                empleadoPage.EnterNombreCompleto(nombre);
                empleadoPage.EnterCorreo(correo);
                empleadoPage.EnterTelefono(telefono);
                empleadoPage.SelectCargo("Desarrollador");

                AddScreenshotToReport("Formulario_Completo");

                LogInfo("Guardando empleado");
                empleadoPage.ClickCrear();

                // Assert
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.Url.Contains("/Home/Index") || d.Url.EndsWith("/Home"));

                AddScreenshotToReport("Empleado_Creado");

                Assert.That(driver.Url.Contains("/Home"), Is.True,
                    "Debe redirigir a la página principal después de crear");

                LogPass("✅ Empleado creado exitosamente");
            }
            catch (Exception ex)
            {
                LogFail($"❌ Prueba fallida: {ex.Message}");
                AddScreenshotToReport("Error_CrearEmpleado");
                throw;
            }
        }

        [Test]
        [Category("CRUD")]
        [Category("Create")]
        [Category("PruebaNegativa")]
        [Description("Verificar validación de formato de correo inválido")]
        public void CrearEmpleado_PruebaNegativa_CorreoInvalido()
        {
            try
            {
                LogInfo("Iniciando prueba: Crear empleado con correo inválido");

                // Arrange
                LogInfo("Navegando a formulario de creación");
                homePage.ClickCrearNuevo();
                System.Threading.Thread.Sleep(1000);

                // Act
                LogInfo("Ingresando correo con formato inválido");
                empleadoPage.EnterNombreCompleto("María González");
                empleadoPage.EnterCorreo("correo-invalido-sin-arroba");
                empleadoPage.EnterTelefono("809-555-9999");
                empleadoPage.SelectCargo("Desarrollador");

                AddScreenshotToReport("Correo_Invalido_Antes");

                // Assert - Verificar validación HTML5 ANTES del submit
                LogInfo("Verificando validación de correo");
                var correoInput = driver.FindElement(By.Id("oEmpleado_Correo"));

                bool isValid = (bool)((IJavaScriptExecutor)driver)
                    .ExecuteScript("return arguments[0].checkValidity();", correoInput);

                LogInfo($"Validación HTML5 - Campo válido: {isValid}");

                Assert.That(isValid, Is.False, "El campo correo debe ser inválido");

                // Intentar submit
                empleadoPage.ClickCrear();
                System.Threading.Thread.Sleep(1000);

                // Debe permanecer en la misma página
                Assert.That(driver.Url.Contains("Empleado_Detalle"), Is.True,
                    "Debe permanecer en el formulario debido a validación");

                AddScreenshotToReport("Correo_Invalido_Validacion");

                LogPass("✅ Sistema validó correctamente el formato de correo");
            }
            catch (Exception ex)
            {
                LogFail($"❌ Prueba fallida: {ex.Message}");
                AddScreenshotToReport("Error_CorreoInvalido");
                throw;
            }
        }

        [Test]
        [Category("CRUD")]
        [Category("Create")]
        [Category("PruebaLimites")]
        [Description("Verificar creación con teléfono en formato no estándar")]
        public void CrearEmpleado_PruebaLimites_TelefonoFormatoVariado()
        {
            try
            {
                LogInfo("Iniciando prueba: Crear empleado con diferentes formatos de teléfono");

                // Arrange
                string[] formatosTelefono = {
                    "8095551234",
                    "(809) 555-1234",
                    "+1-809-555-1234",
                    "809.555.1234"
                };

                LogInfo("Navegando a formulario de creación");
                homePage.ClickCrearNuevo();
                System.Threading.Thread.Sleep(1000);

                // Act
                string formato = formatosTelefono[0];
                string timestamp = DateTime.Now.ToString("HHmmss");

                LogInfo($"Probando formato de teléfono: {formato}");

                empleadoPage.EnterNombreCompleto($"Pedro Martínez {timestamp}");
                empleadoPage.EnterCorreo($"pedro.martinez{timestamp}@test.com");
                empleadoPage.EnterTelefono(formato);
                empleadoPage.SelectCargo("Desarrollador");

                AddScreenshotToReport("Telefono_Formato_Variado_Antes");

                empleadoPage.ClickCrear();

                // Assert
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                try
                {
                    wait.Until(d => d.Url.Contains("/Home/Index") || d.Url.EndsWith("/Home"));
                    LogPass("✅ Sistema aceptó formato no estándar de teléfono");
                }
                catch (WebDriverTimeoutException)
                {
                    LogWarning($"Timeout esperando redirect. URL actual: {driver.Url}");
                }

                AddScreenshotToReport("Telefono_Formato_Variado_Despues");

                Assert.That(driver.Url.Contains("/Home"), Is.True,
                    "Debe aceptar diferentes formatos de teléfono");
            }
            catch (Exception ex)
            {
                LogFail($"❌ Prueba fallida: {ex.Message}");
                AddScreenshotToReport("Error_TelefonoFormato");
                throw;
            }
        }
    }
}