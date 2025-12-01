using NUnit.Framework;
using OpenQA.Selenium;
using CRUDCORE_P3.SeleniumTests.Helpers;
using CRUDCORE_P3.SeleniumTests.PageObjects;
using AventStack.ExtentReports;

namespace CRUDCORE_P3.SeleniumTests.Tests
{
    [TestFixture]
    public class EmpleadoCreateTests : BaseTest
    {
        private LoginPage loginPage;
        private HomePage homePage;
        private EmpleadoDetallePage empleadoPage;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            loginPage = new LoginPage(driver);
            homePage = new HomePage(driver);
            empleadoPage = new EmpleadoDetallePage(driver);
            test = ReportHelper.GetExtent().CreateTest(TestContext.CurrentContext.Test.Name);

            // Login antes de cada prueba
            loginPage.NavigateTo(baseUrl);
            loginPage.Login("admin@test.com", "admin123");
            System.Threading.Thread.Sleep(2000);
        }

        // ================================================================
        // HU-2: Crear Empleado
        // ================================================================

        [Test]
        [Category("CRUD")]
        [Category("Create")]
        [Category("CaminoFeliz")]
        [Description("Verificar que se puede crear un empleado con datos válidos")]
        public void CrearEmpleado_CaminoFeliz_DatosValidos()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Crear empleado con datos válidos");

                // Arrange
                string nombre = "Juan Carlos Pérez";
                string correo = "juan.perez@company.com";
                string telefono = "809-555-1234";
                string cargo = "Desarrollador"; // Asegúrate que este cargo existe en tu BD

                test.Log(Status.Info, "Navegando a formulario de creación");
                homePage.ClickCrearNuevo();
                System.Threading.Thread.Sleep(1000);

                // Act
                test.Log(Status.Info, $"Creando empleado: {nombre}");
                empleadoPage.CreateEmpleado(nombre, correo, telefono, cargo);
                System.Threading.Thread.Sleep(2000);

                // Assert
                test.Log(Status.Info, "Verificando que el empleado fue creado");
                Assert.That(homePage.IsEmployeeDisplayed(nombre), Is.True,
                    "El empleado debe aparecer en la lista");

                ScreenshotHelper.TakeScreenshot(driver, "Crear_Empleado_Exitoso");
                test.Log(Status.Pass, "Empleado creado exitosamente");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
                throw;
            }
        }

        [Test]
        [Category("CRUD")]
        [Category("Create")]
        [Category("PruebaNegativa")]
        [Description("Verificar que no se puede crear empleado sin nombre")]
        public void CrearEmpleado_PruebaNegativa_SinNombre()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Intentar crear empleado sin nombre");

                // Arrange
                test.Log(Status.Info, "Navegando a formulario de creación");
                homePage.ClickCrearNuevo();
                System.Threading.Thread.Sleep(1000);

                // Act - Intentar crear sin nombre
                test.Log(Status.Info, "Intentando crear empleado sin nombre");
                empleadoPage.EnterNombreCompleto("");
                empleadoPage.EnterCorreo("test@test.com");
                empleadoPage.EnterTelefono("809-555-0000");
                empleadoPage.SelectCargo("Desarrollador");
                empleadoPage.ClickCrear();

                System.Threading.Thread.Sleep(1000);

                // Assert - Debe quedarse en la misma página o mostrar error
                test.Log(Status.Info, "Verificando que no se creó el empleado");
                Assert.That(driver.Url.Contains("/Empleado_Detalle"), Is.True,
                    "No debe crear empleado sin nombre obligatorio");

                ScreenshotHelper.TakeScreenshot(driver, "Crear_Empleado_Sin_Nombre");
                test.Log(Status.Pass, "El sistema rechazó correctamente empleado sin nombre");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
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
                test.Log(Status.Info, "Iniciando prueba: Crear empleado con correo inválido");

                // Arrange
                test.Log(Status.Info, "Navegando a formulario de creación");
                homePage.ClickCrearNuevo();
                System.Threading.Thread.Sleep(1000);

                // Act - Correo sin formato válido
                test.Log(Status.Info, "Ingresando correo con formato inválido");
                empleadoPage.EnterNombreCompleto("María González");
                empleadoPage.EnterCorreo("correo-invalido-sin-arroba");
                empleadoPage.EnterTelefono("809-555-9999");
                empleadoPage.SelectCargo("Desarrollador");

                // Tomar screenshot antes de hacer clic
                ScreenshotHelper.TakeScreenshot(driver, "Crear_Empleado_Correo_Invalido_Antes");

                empleadoPage.ClickCrear();
                System.Threading.Thread.Sleep(1000);

                // Assert - El navegador debe mostrar validación HTML5
                test.Log(Status.Info, "Verificando validación de correo");
                var correoInput = driver.FindElement(By.Id("oEmpleado_Correo"));
                bool isValid = (bool)((IJavaScriptExecutor)driver)
                    .ExecuteScript("return arguments[0].validity.valid;", correoInput);

                Assert.That(isValid, Is.False,
                    "El campo correo debe ser inválido");

                ScreenshotHelper.TakeScreenshot(driver, "Crear_Empleado_Correo_Invalido");
                test.Log(Status.Pass, "El sistema validó correctamente el formato de correo");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
                throw;
            }
        }

        [Test]
        [Category("CRUD")]
        [Category("Create")]
        [Category("PruebaLimites")]
        [Description("Verificar creación con nombre en el límite de caracteres")]
        public void CrearEmpleado_PruebaLimites_NombreLongitudMaxima()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Crear empleado con nombre de 60 caracteres");

                // Arrange - Nombre de 60 caracteres (límite de la BD)
                string nombreLargo = new string('A', 60);
                string correo = "limite@test.com";
                string telefono = "809-111-2222";

                test.Log(Status.Info, "Navegando a formulario de creación");
                homePage.ClickCrearNuevo();
                System.Threading.Thread.Sleep(1000);

                // Act
                test.Log(Status.Info, $"Creando empleado con nombre de {nombreLargo.Length} caracteres");
                empleadoPage.CreateEmpleado(nombreLargo, correo, telefono, "Desarrollador");
                System.Threading.Thread.Sleep(2000);

                // Assert
                test.Log(Status.Info, "Verificando que aceptó el límite de caracteres");
                Assert.That(driver.Url.Contains("/Home/Index"), Is.True,
                    "Debe aceptar nombre con longitud máxima");

                ScreenshotHelper.TakeScreenshot(driver, "Crear_Empleado_Nombre_Longitud_Maxima");
                test.Log(Status.Pass, "El sistema aceptó nombre en el límite de caracteres");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
                throw;
            }
        }

        [Test]
        [Category("CRUD")]
        [Category("Create")]
        [Category("PruebaLimites")]
        [Description("Verificar creación con caracteres especiales en nombre")]
        public void CrearEmpleado_PruebaLimites_CaracteresEspeciales()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Crear empleado con caracteres especiales");

                // Arrange
                string nombreEspecial = "José María O'Connor-López";
                string correo = "jose.maria@test.com";
                string telefono = "809-333-4444";

                test.Log(Status.Info, "Navegando a formulario de creación");
                homePage.ClickCrearNuevo();
                System.Threading.Thread.Sleep(1000);

                // Act
                test.Log(Status.Info, $"Creando empleado: {nombreEspecial}");
                empleadoPage.CreateEmpleado(nombreEspecial, correo, telefono, "Desarrollador");
                System.Threading.Thread.Sleep(2000);

                // Assert
                test.Log(Status.Info, "Verificando que aceptó caracteres especiales");
                Assert.That(homePage.IsEmployeeDisplayed(nombreEspecial), Is.True,
                    "Debe aceptar nombres con acentos y caracteres especiales");

                ScreenshotHelper.TakeScreenshot(driver, "Crear_Empleado_Caracteres_Especiales");
                test.Log(Status.Pass, "El sistema aceptó caracteres especiales en nombre");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
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
                test.Log(Status.Info, "Iniciando prueba: Crear empleado con diferentes formatos de teléfono");

                // Arrange - Diferentes formatos de teléfono
                string[] formatosTelefono = {
                    "8095551234",           // Sin guiones
                    "(809) 555-1234",       // Con paréntesis
                    "+1-809-555-1234",      // Con código país
                    "809.555.1234"          // Con puntos
                };

                test.Log(Status.Info, "Navegando a formulario de creación");
                homePage.ClickCrearNuevo();
                System.Threading.Thread.Sleep(1000);

                // Act - Probar primer formato
                string formato = formatosTelefono[0];
                test.Log(Status.Info, $"Probando formato de teléfono: {formato}");
                empleadoPage.EnterNombreCompleto("Pedro Martínez");
                empleadoPage.EnterCorreo("pedro.martinez@test.com");
                empleadoPage.EnterTelefono(formato);
                empleadoPage.SelectCargo("Desarrollador");

                ScreenshotHelper.TakeScreenshot(driver, "Crear_Empleado_Telefono_Formato_Variado");

                empleadoPage.ClickCrear();
                System.Threading.Thread.Sleep(2000);

                // Assert
                test.Log(Status.Info, "Verificando que aceptó formato de teléfono");
                Assert.That(driver.Url.Contains("/Home/Index"), Is.True,
                    "Debe aceptar diferentes formatos de teléfono");

                test.Log(Status.Pass, "El sistema aceptó formato no estándar de teléfono");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
                throw;
            }
        }
    }
}