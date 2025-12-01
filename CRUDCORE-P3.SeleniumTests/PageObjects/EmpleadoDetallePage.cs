using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CRUDCORE_P3.SeleniumTests.PageObjects
{
    public class EmpleadoDetallePage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        // SELECTORES CORREGIDOS
        private By nombreInput = By.Name("oEmpleado.NombreCompleto");
        private By correoInput = By.Name("oEmpleado.Correo");
        private By telefonoInput = By.Name("oEmpleado.Telefono");
        private By cargoSelect = By.Name("oEmpleado.IdCargo");
        private By crearButton = By.CssSelector("button[type='submit']");
        private By actualizarButton = By.CssSelector("button[type='submit']");
        private By volverButton = By.LinkText("Volvers");

        public EmpleadoDetallePage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void EnterNombreCompleto(string nombre)
        {
            var element = wait.Until(d => d.FindElement(nombreInput));
            element.Clear();
            element.SendKeys(nombre);
        }

        public void EnterCorreo(string correo)
        {
            var element = driver.FindElement(correoInput);
            element.Clear();
            element.SendKeys(correo);
        }

        public void EnterTelefono(string telefono)
        {
            var element = driver.FindElement(telefonoInput);
            element.Clear();
            element.SendKeys(telefono);
        }

        public void SelectCargo(string cargoText)
        {
            var select = new SelectElement(driver.FindElement(cargoSelect));
            select.SelectByText(cargoText);
        }

        public void ClickCrear()
        {
            driver.FindElement(crearButton).Click();
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
            System.Threading.Thread.Sleep(500); // Pequeña pausa
            ClickCrear();
        }

        public void UpdateEmpleado(string nombre, string correo, string telefono, string cargo)
        {
            EnterNombreCompleto(nombre);
            EnterCorreo(correo);
            EnterTelefono(telefono);
            SelectCargo(cargo);
            System.Threading.Thread.Sleep(500);
            ClickActualizar();
        }
    }
}