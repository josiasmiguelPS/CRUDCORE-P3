using AventStack.ExtentReports;
using CRUDCORE_P3.SeleniumTests.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;

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

            // Crear el test en ExtentReports
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name)
                .AssignCategory(TestContext.CurrentContext.Test.Properties["Category"].Cast<string>().FirstOrDefault() ?? "General")
                .AssignAuthor("Tu Nombre Aquí");

            loginPage = new LoginPage(driver);
        }

        [Test]
        [Category("Login")]
        [Category("CaminoFeliz")]
        [Description("Verificar que un usuario puede iniciar sesión con credenciales válidas")]
        public void Login_CaminoFeliz_CredencialesValidas()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Login exitoso con credenciales válidas");

                string correo = "admin@test.com";
                string password = "admin123";

                test.Log(Status.Info, $"Navegando a: {baseUrl}");
                loginPage.NavigateTo(baseUrl);

                AddScreenshotToReport("Login_PaginaInicial");

                test.Log(Status.Info, $"Ingresando credenciales - Correo: {correo}");
                loginPage.Login(correo, password);

                System.Threading.Thread.Sleep(2000);

                test.Log(Status.Info, "Verificando redirección a Home");
                Assert.That(driver.Url.Contains("/Home"), Is.True,
                    "El usuario debería ser redirigido a la página principal");

                AddScreenshotToReport("Login_Exitoso");
                test.Pass("✅ Login exitoso - Usuario autenticado correctamente");
            }
            catch (Exception ex)
            {
                test.Fail($"❌ Prueba fallida: {ex.Message}");
                AddScreenshotToReport("Login_Error");
                throw;
            }
        }

        [Test]
        [Category("Login")]
        [Category("PruebaNegativa")]
        [Description("Verificar que el sistema rechaza credenciales incorrectas")]
        public void Login_PruebaNegativa_CredencialesIncorrectas()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Login con credenciales incorrectas");

                loginPage.NavigateTo(baseUrl);

                test.Log(Status.Info, "Ingresando credenciales inválidas");
                loginPage.Login("usuario@invalido.com", "password_incorrecta");

                System.Threading.Thread.Sleep(1000);

                AddScreenshotToReport("Login_CredencialesIncorrectas");

                test.Log(Status.Info, "Verificando que permanece en login");
                Assert.That(driver.Url.Contains("/Login"), Is.True,
                    "Debe permanecer en la página de login");

                // Verificar mensaje de error
                var errorMsg = driver.FindElement(By.XPath("//*[contains(text(), 'Credenciales incorrectas')]"));
                Assert.That(errorMsg.Displayed, Is.True, "Debe mostrar mensaje de error");

                test.Pass("✅ Sistema rechazó correctamente las credenciales inválidas");
            }
            catch (Exception ex)
            {
                test.Fail($"❌ Prueba fallida: {ex.Message}");
                AddScreenshotToReport("Error_CredencialesIncorrectas");
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

                loginPage.NavigateTo(baseUrl);
                System.Threading.Thread.Sleep(1000);

                string urlInicial = driver.Url;
                test.Log(Status.Info, $"URL inicial: {urlInicial}");

                // Verificar atributos HTML5
                var correoInput = driver.FindElement(By.Name("correo"));
                var passwordInput = driver.FindElement(By.Name("password"));

                string correoRequired = correoInput.GetAttribute("required");
                string correoType = correoInput.GetAttribute("type");

                test.Log(Status.Info, $"Campo correo - Type: {correoType}, Required: {correoRequired}");

                loginPage.EnterCorreo("");
                loginPage.EnterPassword("");

                AddScreenshotToReport("Login_CamposVacios_Antes");

                bool correoValido = (bool)((IJavaScriptExecutor)driver)
                    .ExecuteScript("return arguments[0].checkValidity();", correoInput);

                Assert.That(correoValido, Is.False, "Campo correo vacío debe ser inválido");

                loginPage.ClickLogin();
                System.Threading.Thread.Sleep(1000);

                AddScreenshotToReport("Login_CamposVacios_Validacion");

                Assert.That(driver.Url, Is.EqualTo(urlInicial),
                    "No debería redirigir con campos vacíos");

                test.Pass("✅ Sistema validó correctamente campos vacíos");
            }
            catch (Exception ex)
            {
                test.Fail($"❌ Prueba fallida: {ex.Message}");
                AddScreenshotToReport("Error_CamposVacios");
                throw;
            }
        }

        [Test]
        [Category("Login")]
        [Category("Seguridad")]
        [Description("Verificar protección contra inyección SQL")]
        public void Login_Seguridad_InyeccionSQL()
        {
            try
            {
                test.Log(Status.Info, "Iniciando prueba: Prevención de inyección SQL");
                test.Log(Status.Warning, "⚠️ Prueba de seguridad - Intentando inyección SQL");

                loginPage.NavigateTo(baseUrl);

                // Bypass validación HTML5 con JavaScript
                var correoInput = driver.FindElement(By.Name("correo"));
                var passwordInput = driver.FindElement(By.Name("password"));

                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].value = arguments[1]; arguments[0].type = 'text';",
                    correoInput, "admin@test.com' OR '1'='1");
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].value = arguments[1];",
                    passwordInput, "' OR '1'='1");

                test.Log(Status.Info, "Valores de inyección SQL establecidos");

                AddScreenshotToReport("Login_InyeccionSQL_Antes");

                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].submit();",
                    driver.FindElement(By.TagName("form")));

                System.Threading.Thread.Sleep(2000);

                AddScreenshotToReport("Login_InyeccionSQL_Despues");

                bool enLogin = driver.Url.Contains("/Login");
                Assert.That(enLogin, Is.True,
                    "Sistema debe rechazar inyección SQL");

                test.Pass("✅ Sistema protegido contra inyección SQL");
            }
            catch (Exception ex)
            {
                test.Fail($"❌ Prueba fallida: {ex.Message}");
                AddScreenshotToReport("Error_InyeccionSQL");
                throw;
            }
        }
    }
}