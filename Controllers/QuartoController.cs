using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoHotelJoao.Data;
using GestaoHotelJoao.Models;


namespace GestaoHotelJoao.Controllers
{
    public class QuartoController : Controller
    {
        private readonly DbGestaoHotelJoaoContext _context;

        public QuartoController(DbGestaoHotelJoaoContext context)
        {
            _context = context;
        }

        // GET: Quarto
        public async Task<IActionResult> Index()
        {
            var isAdmin = HttpContext.Session.GetString("ADMIN");
            var utilizador = HttpContext.Session.GetString("UTILIZADOR");

            if (isAdmin == "true" &&  HttpContext.Session.GetString("LOGADO") == "true")
            {
                return View(await _context.Quartos.ToListAsync());
            }
            else
            {
                return RedirectToAction("Index", "Login");
            } 
        }

        // GET: Quarto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quarto = await _context.Quartos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quarto == null)
            {
                return NotFound();
            }

            return View(quarto);
        }

        // GET: Quarto/Create
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

        // POST: Quarto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoQuarto,CustoNoite")] Quarto quarto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quarto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quarto);
        }

        // GET: Quarto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var isAdmin = HttpContext.Session.GetString("ADMIN");

            if (isAdmin == "true" &&  HttpContext.Session.GetString("LOGADO") == "true")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var quarto = await _context.Quartos.FindAsync(id);
                if (quarto == null)
                {
                    return NotFound();
                }
                return View(quarto);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: Quarto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TipoQuarto,CustoNoite")] Quarto quarto)
        {
            if (id != quarto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quarto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuartoExists(quarto.Id))
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
            return View(quarto);
        }

        // GET: Quarto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var isAdmin = HttpContext.Session.GetString("ADMIN");

            if (isAdmin == "true" &&  HttpContext.Session.GetString("LOGADO") == "true")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var quarto = await _context.Quartos
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (quarto == null)
                {
                    return NotFound();
                }

                return View(quarto);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: Quarto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quarto = await _context.Quartos.FindAsync(id);
            if (quarto != null)
            {
                _context.Quartos.Remove(quarto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuartoExists(int id)
        {
            return _context.Quartos.Any(e => e.Id == id);
        }
    }
}
