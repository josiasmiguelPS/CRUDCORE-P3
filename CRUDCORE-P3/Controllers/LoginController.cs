using CRUDCORE_P3.Models;
using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly DbcrudcoreContext _context;

    public LoginController(DbcrudcoreContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string correo, string password)
    {
        var usuario = _context.Usuarios
            .FirstOrDefault(u => u.Correo == correo && u.Password == password);

        if (usuario != null)
        {
            HttpContext.Session.SetString("usuario", usuario.Nombre);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Credenciales incorrectas";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
