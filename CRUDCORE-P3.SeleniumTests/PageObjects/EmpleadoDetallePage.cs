using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CRUDCORE_P3.SeleniumTests.PageObjects
{
    public class EmpleadoDetallePage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private By nombreInput = By.Id("oEmpleado_NombreCompleto");
        private By correoInput = By.Id("oEmpleado_Correo");
        private By telefonoInput = By.Id("oEmpleado_Telefono");
        private By cargoSelect = By.Id("oEmpleado_IdCargo");
        private By crearButton = By.XPath("//button[contains(text(),'Crear')]");
        private By actualizarButton = By.XPath("//button[text()='Actualizar']");
        private By volverButton = By.LinkText("Volvers");

        public EmpleadoDetallePage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void EnterNombreCompleto(string nombre)
        {
            wait.Until(d => d.FindElement(nombreInput)).Clear();
            driver.FindElement(nombreInput).SendKeys(nombre);
        }

        public void EnterCorreo(string correo)
        {
            driver.FindElement(correoInput).Clear();
            driver.FindElement(correoInput).SendKeys(correo);
        }

        public void EnterTelefono(string telefono)
        {
            driver.FindElement(telefonoInput).Clear();
            driver.FindElement(telefonoInput).SendKeys(telefono);
        }

        public void SelectCargo(string cargoText)
        {
            var select = new SelectElement(driver.FindElement(cargoSelect));
            select.SelectByText(cargoText);
        }

        public void ClickCrear()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var botonCrear = wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']")));

            // Scroll al botón si es necesario
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", botonCrear);
            System.Threading.Thread.Sleep(500);

            botonCrear.Click();
        }

        public void ClickActualizar()
        {
            driver.FindElement(actualizarButton).Click();
        }

        public void CreateEmpleado(string nombre, string correo, string telefono, string cargo)
        {
            EnterNombreCompleto(nombre);
            EnterCorreo(correo);
            EnterTelefono(telefono);
            SelectCargo(cargo);
            ClickCrear();
        }

        public void UpdateEmpleado(string nombre, string correo, string telefono, string cargo)
        {
            EnterNombreCompleto(nombre);
            EnterCorreo(correo);
            EnterTelefono(telefono);
            SelectCargo(cargo);
            ClickActualizar();
        }
    }
}