using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoHotelJoao.Data;
using GestaoHotelJoao.Models;

namespace GestaoHotelJoao.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly DbGestaoHotelJoaoContext _context;

        public FuncionarioController(DbGestaoHotelJoaoContext context)
        {
            _context = context;
        }

        // GET: Funcionario
        public async Task<IActionResult> Index()
        {
            var isAdmin = HttpContext.Session.GetString("ADMIN");


            if (isAdmin == "true" && HttpContext.Session.GetString("LOGADO") == "true")
            {
                return View(await _context.Funcionarios.ToListAsync());
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // GET: Funcionario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var isAdmin = HttpContext.Session.GetString("ADMIN");


            if (isAdmin == "true" && HttpContext.Session.GetString("LOGADO") == "true")
            {


                if (id == null)
                {
                    return NotFound();
                }

                var funcionario = await _context.Funcionarios
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (funcionario == null)
                {
                    return NotFound();
                }

                return View(funcionario);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // GET: Funcionario/Create
        public IActionResult Create()
        {
            var isAdmin = HttpContext.Session.GetString("ADMIN");


            if (isAdmin == "true" && HttpContext.Session.GetString("LOGADO") == "true")
            {

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        // POST: Funcionario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funcionario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(funcionario);
        }

        // GET: Funcionario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var isAdmin = HttpContext.Session.GetString("ADMIN");


            if (isAdmin == "true" && HttpContext.Session.GetString("LOGADO") == "true")
            {


                if (id == null)
                {
                    return NotFound();
                }

                var funcionario = await _context.Funcionarios.FindAsync(id);
                if (funcionario == null)
                {
                    return NotFound();
                }
                return View(funcionario);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: Funcionario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Funcionario funcionario)
        {
            if (id != funcionario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcionario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionarioExists(funcionario.Id))
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
            return View(funcionario);
        }

        // GET: Funcionario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            var isAdmin = HttpContext.Session.GetString("ADMIN");


            if (isAdmin == "true" && HttpContext.Session.GetString("LOGADO") == "true")
            {

                if (id == null)
                {
                    return NotFound();
                }

                var funcionario = await _context.Funcionarios
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (funcionario == null)
                {
                    return NotFound();
                }

                return View(funcionario);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: Funcionario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funcionario = await _context.Funcionarios
                                            .Include(f => f.Registos)
                                            .FirstOrDefaultAsync(m => m.Id == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            if (funcionario.Registos.Any())
            {
                ViewBag.ErrorMessage = "Este funcionário tem registos associados, não pode ser apagado.";
                return RedirectToAction(nameof(Index));
            }

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool FuncionarioExists(int id)
        {
            return _context.Funcionarios.Any(e => e.Id == id);
        }
    }
}
