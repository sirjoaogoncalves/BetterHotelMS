using Microsoft.AspNetCore.Mvc;

namespace GestaoHotelJoao.Controllers
{
    public class AdminAreaController : Controller
    {
       
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" &&  HttpContext.Session.GetString("LOGADO") == "true")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
