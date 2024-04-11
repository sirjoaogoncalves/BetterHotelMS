using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoHotelJoao.Data;
using GestaoHotelJoao.Models;

namespace GestaoHotelJoao.Controllers
{
    public class UserController : Controller
    {
        private readonly DbGestaoHotelJoaoContext _context;

        public UserController(DbGestaoHotelJoaoContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" &&  HttpContext.Session.GetString("LOGADO") == "true")
            {
            return View(await _context.Users.ToListAsync());
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(HttpContext.Session.GetString("ADMIN") == "true" &&  HttpContext.Session.GetString("LOGADO") == "true")
            {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // GET: User/Create
        public IActionResult Create()
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

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Senha,Administrador,Funcionario")] User user)
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" &&  HttpContext.Session.GetString("LOGADO") == "true")
            {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" &&  HttpContext.Session.GetString("LOGADO") == "true")
            {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Senha,Administrador,Funcionario")] User user)
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" &&  HttpContext.Session.GetString("LOGADO") == "true")
            {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" &&  HttpContext.Session.GetString("LOGADO") == "true")
            {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" &&  HttpContext.Session.GetString("LOGADO") == "true")
            {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
