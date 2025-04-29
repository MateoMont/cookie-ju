using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebGalletas.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string clave)
        {
            if (clave == "tupasswordsupersecreta")
            {
                HttpContext.Session.SetString("EsAdmin", "true");
                return RedirectToAction("Index", "Home"); // Redirige al inicio o donde prefieras
            }

            TempData["Error"] = "Clave incorrecta";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("EsAdmin");
            return RedirectToAction("Index", "Home");
        }
    }
}
