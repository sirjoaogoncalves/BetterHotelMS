using Microsoft.AspNetCore.Mvc;
using GestaoHotelJoao.Data;

namespace GestaoHotelJoao.Controllers
{
    public class LoginController : Controller
    {
        private readonly DbGestaoHotelJoaoContext _context;

        public LoginController(DbGestaoHotelJoaoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(string username, string password)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Nome de utilizador ou palavra-passe vazios.";
                return View();
            }


            var user = _context.Users.FirstOrDefault(t => t.Username == username && t.Senha == password);


            if (user == null)
            {
                ViewBag.ErrorMessage = "Nao existe nenhum utilizador.";
                return View();
            }


            if (user.Administrador == true)
            {
                HttpContext.Session.SetString("UTILIZADOR", user.Username);
                HttpContext.Session.SetString("ADMIN", "true");
                HttpContext.Session.SetString("FUNCIONARIO", "false");
                HttpContext.Session.SetString("LOGADO", "true");
                return RedirectToAction("Index", "Home");
            }
            else if (user.Administrador == false && user.Funcionario == true)
            {
                HttpContext.Session.SetString("UTILIZADOR", user.Username);
                HttpContext.Session.SetString("ADMIN", "false");
                HttpContext.Session.SetString("FUNCIONARIO", "true");
                HttpContext.Session.SetString("LOGADO", "true");        
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }



            ViewBag.ErrorMessage = "Nao existe nenhum utilizador.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
