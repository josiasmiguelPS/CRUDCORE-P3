using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRUDCORE_P3.SeleniumTests.PageObjects
{
    public class HomePage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private By crearNuevoButton = By.LinkText("Crear Nuevo");
        private By employeeTable = By.CssSelector("table.table");
        private By editarButtons = By.LinkText("Editar");
        private By eliminarButtons = By.LinkText("Eliminar");

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void ClickCrearNuevo()
        {
            wait.Until(d => d.FindElement(crearNuevoButton)).Click();
        }

        public bool IsTableDisplayed()
        {
            try
            {
                return wait.Until(d => d.FindElement(employeeTable).Displayed);
            }
            catch
            {
                return false;
            }
        }

        public int GetEmployeeCount()
        {
            var rows = driver.FindElements(By.CssSelector("table tbody tr"));
            return rows.Count;
        }

        public void ClickEditarByName(string nombreCompleto)
        {
            var row = driver.FindElement(By.CssSelector($"td:contains('{nombreCompleto}')"));

            row.FindElement(By.LinkText("Editar")).Click();
        }

        public void ClickEliminarByName(string nombreCompleto)
        {
            var row = driver.FindElement(By.CssSelector($"td:contains('{nombreCompleto}')"));

            row.FindElement(By.LinkText("Eliminar")).Click();
        }

        public bool IsEmployeeDisplayed(string nombreCompleto)
        {
            try
            {
                // Escapa apóstrofes para XPath
                string escapedNombre = nombreCompleto.Contains("'")
                    ? $"concat('{nombreCompleto.Replace("'", "',\"'\",'")}')"
                    : $"'{nombreCompleto}'";

                driver.FindElement(By.XPath($"//td[text()={escapedNombre}]"));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

    }
}