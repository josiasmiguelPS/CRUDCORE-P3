using CRUDCORE_P3.Models;
using CRUDCORE_P3_Prueva.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CRUDCORE_P3_Prueva.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbcrudcoreContext _DBContext;

        public HomeController(DbcrudcoreContext context)
        {
            _DBContext = context;
        }

        public IActionResult Index()
        {
            List<Empleado> list = _DBContext.Empleados.Include(c => c.oCargo).ToList();
            return View(list);
        }
    }
}
