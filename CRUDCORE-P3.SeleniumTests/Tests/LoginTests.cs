using NUnit.Framework;
using OpenQA.Selenium;
using CRUDCORE_P3.SeleniumTests.Helpers;
using CRUDCORE_P3.SeleniumTests.PageObjects;
using AventStack.ExtentReports;

namespace CRUDCORE_P3.SeleniumTests.Tests
{
    [TestFixture]
    public class LoginTests : BaseTest
    {
        private LoginPage loginPage;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            loginPage = new LoginPage(driver);
            test = ReportHelper.GetExtent().CreateTest(TestContext.CurrentContext.Test.Name);
        }

        // ================================================================
        // HU-1: Login de Usuario
        // ================================================================

        [Test]
        [Category("Login")]
        [Category("CaminoFeliz")]
        [Description("Verificar que un usuario puede iniciar sesión con credenciales válidas")]
        public void Login_CaminoFeliz_CredencialesValidas()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Login exitoso con credenciales válidas");

                // Arrange
                string correo = "admin@test.com";
                string password = "admin123";

                test.Log(Status.Info, $"Navegando a: {baseUrl}");
                loginPage.NavigateTo(baseUrl);

                // Act
                test.Log(Status.Info, $"Ingresando credenciales - Correo: {correo}");
                loginPage.Login(correo, password);

                // Wait for redirect
                System.Threading.Thread.Sleep(2000);

                // Assert
                test.Log(Status.Info, "Verificando redirección a Home");
                Assert.That(driver.Url.Contains("/Home/Index"), Is.True,
                    "El usuario debería ser redirigido a la página principal");

                ScreenshotHelper.TakeScreenshot(driver, "Login_Exitoso");
                test.Log(Status.Pass, "Login exitoso - Usuario autenticado correctamente");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
                throw;
            }
        }

        [Test]
        [Category("Login")]
        [Category("PruebaNegativa")]
        [Description("Verificar que el sistema rechaza credenciales inválidas")]
        public void Login_PruebaNegativa_CredencialesInvalidas()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Login con credenciales inválidas");

                // Arrange
                string correo = "usuario@invalido.com";
                string password = "passwordincorrecto";

                test.Log(Status.Info, $"Navegando a: {baseUrl}");
                loginPage.NavigateTo(baseUrl);

                // Act
                test.Log(Status.Info, $"Intentando login con credenciales inválidas");
                loginPage.Login(correo, password);

                System.Threading.Thread.Sleep(1000);

                // Assert
                test.Log(Status.Info, "Verificando mensaje de error");
                Assert.That(loginPage.IsErrorMessageDisplayed(), Is.True,
                    "Debería mostrar mensaje de error con credenciales inválidas");

                Assert.That(loginPage.GetErrorMessage(),
                    Does.Contain("Credenciales incorrectas"),
                    "El mensaje de error debe indicar credenciales incorrectas");

                ScreenshotHelper.TakeScreenshot(driver, "Login_Credenciales_Invalidas");
                test.Log(Status.Pass, "El sistema rechazó correctamente las credenciales inválidas");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
                throw;
            }
        }

        [Test]
        [Category("Login")]
        [Category("PruebaNegativa")]
        [Description("Verificar que el sistema rechaza campos vacíos")]
        public void Login_PruebaNegativa_CamposVacios()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Login con campos vacíos");

                // Arrange
                test.Log(Status.Info, $"Navegando a: {baseUrl}");
                loginPage.NavigateTo(baseUrl);

                // Act - Intentar login sin llenar campos
                test.Log(Status.Info, "Intentando login con campos vacíos");
                loginPage.EnterCorreo("");
                loginPage.EnterPassword("");
                loginPage.ClickLogin();

                System.Threading.Thread.Sleep(1000);

                // Assert - Verificar que no se redirige
                test.Log(Status.Info, "Verificando que permanece en página de login");
                Assert.That(driver.Url.Contains("/Login"), Is.True,
                    "No debería redirigir con campos vacíos");

                ScreenshotHelper.TakeScreenshot(driver, "Login_Campos_Vacios");
                test.Log(Status.Pass, "El sistema rechazó correctamente campos vacíos");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
                throw;
            }
        }

        [Test]
        [Category("Login")]
        [Category("PruebaLimites")]
        [Description("Verificar comportamiento con correo de longitud máxima")]
        public void Login_PruebaLimites_CorreoLongitudMaxima()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Login con correo de longitud máxima");

                // Arrange - Correo de 60 caracteres (límite de la BD)
                string correoLargo = new string('a', 48) + "@example.com"; // 60 chars
                string password = "test123";

                test.Log(Status.Info, $"Navegando a: {baseUrl}");
                loginPage.NavigateTo(baseUrl);

                // Act
                test.Log(Status.Info, $"Intentando login con correo largo ({correoLargo.Length} caracteres)");
                loginPage.Login(correoLargo, password);

                System.Threading.Thread.Sleep(1000);

                // Assert - Debe aceptar el correo largo pero fallar autenticación
                test.Log(Status.Info, "Verificando que el sistema procesa correos largos");
                Assert.That(loginPage.IsErrorMessageDisplayed(), Is.True,
                    "Debería procesar correo largo y mostrar error de autenticación");

                ScreenshotHelper.TakeScreenshot(driver, "Login_Correo_Longitud_Maxima");
                test.Log(Status.Pass, "El sistema maneja correctamente correos de longitud máxima");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
                throw;
            }
        }

        [Test]
        [Category("Login")]
        [Category("PruebaLimites")]
        [Description("Verificar comportamiento con caracteres especiales en contraseña")]
        public void Login_PruebaLimites_CaracteresEspecialesPassword()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Login con caracteres especiales en password");

                // Arrange
                string correo = "test@test.com";
                string passwordEspecial = "P@ssw0rd!#$%&*()";

                test.Log(Status.Info, $"Navegando a: {baseUrl}");
                loginPage.NavigateTo(baseUrl);

                // Act
                test.Log(Status.Info, "Intentando login con password con caracteres especiales");
                loginPage.Login(correo, passwordEspecial);

                System.Threading.Thread.Sleep(1000);

                // Assert
                test.Log(Status.Info, "Verificando que el sistema acepta caracteres especiales");
                // El sistema debe procesar la contraseña sin errores
                Assert.DoesNotThrow(() => loginPage.ClickLogin(),
                    "El sistema debe aceptar caracteres especiales en password");

                ScreenshotHelper.TakeScreenshot(driver, "Login_Password_Caracteres_Especiales");
                test.Log(Status.Pass, "El sistema maneja correctamente caracteres especiales");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
                throw;
            }
        }

        [Test]
        [Category("Login")]
        [Category("PruebaNegativa")]
        [Description("Verificar que el sistema rechaza inyección SQL en login")]
        public void Login_Seguridad_InyeccionSQL()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Prevención de inyección SQL");

                // Arrange - Intentar inyección SQL común
                string correoMalicioso = "admin' OR '1'='1";
                string passwordMalicioso = "' OR '1'='1";

                test.Log(Status.Info, $"Navegando a: {baseUrl}");
                loginPage.NavigateTo(baseUrl);

                // Act
                test.Log(Status.Warning, "Intentando inyección SQL (prueba de seguridad)");
                loginPage.Login(correoMalicioso, passwordMalicioso);

                System.Threading.Thread.Sleep(1000);

                // Assert - NO debe permitir acceso
                test.Log(Status.Info, "Verificando que el sistema bloquea inyección SQL");
                Assert.That(driver.Url.Contains("/Login"), Is.True,
                    "El sistema NO debe autenticar con inyección SQL");

                ScreenshotHelper.TakeScreenshot(driver, "Login_Seguridad_SQL_Injection");
                test.Log(Status.Pass, "El sistema está protegido contra inyección SQL");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"Prueba fallida: {ex.Message}");
                throw;
            }
        }

    }
}