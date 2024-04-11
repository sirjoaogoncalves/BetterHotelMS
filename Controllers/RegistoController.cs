using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoHotelJoao.Data;
using GestaoHotelJoao.Models;
using System.Text;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace GestaoHotelJoao.Controllers
{
    public class RegistoController : Controller
    {
        private readonly DbGestaoHotelJoaoContext _context;

        public RegistoController(DbGestaoHotelJoaoContext context)
        {
            _context = context;
        }

        // GET: Registo
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" || HttpContext.Session.GetString("LOGADO") == "true")
            {
            var dbGestaoHotelJoaoContext = _context.Registos.Include(r => r.Cliente).Include(r => r.Funcionario).Include(r => r.Quarto);
            return View(await dbGestaoHotelJoaoContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // GET: Registo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registo = await _context.Registos
                .Include(r => r.Cliente)
                .Include(r => r.Funcionario)
                .Include(r => r.Quarto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registo == null)
            {
                return NotFound();
            }

            return View(registo);
        }

        // GET: Registo/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" || HttpContext.Session.GetString("LOGADO") == "true")
            {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome");
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Nome");
            ViewData["QuartoId"] = new SelectList(_context.Quartos, "Id", "TipoQuarto");
            return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: Registo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CheckIn,CheckOut,QuartoId,ClienteId,FuncionarioId,TotalDiasEstadia")] Registo registo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", registo.ClienteId);
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Id", registo.FuncionarioId);
            ViewData["QuartoId"] = new SelectList(_context.Quartos, "Id", "Id", registo.QuartoId);
            return View(registo);
        }

        // GET: Registo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" || HttpContext.Session.GetString("LOGADO") == "true")
            {

            if (id == null)
            {
                return NotFound();
            }

            var registo = await _context.Registos.FindAsync(id);
            if (registo == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", registo.ClienteId);
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Nome", registo.FuncionarioId);
            ViewData["QuartoId"] = new SelectList(_context.Quartos, "Id", "TipoQuarto", registo.QuartoId);
            return View(registo);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: Registo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CheckIn,CheckOut,QuartoId,ClienteId,FuncionarioId,TotalDiasEstadia")] Registo registo)
        {
            if (id != registo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistoExists(registo.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", registo.ClienteId);
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Id", registo.FuncionarioId);
            ViewData["QuartoId"] = new SelectList(_context.Quartos, "Id", "Id", registo.QuartoId);
            return View(registo);
        }

        // GET: Registo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" || HttpContext.Session.GetString("LOGADO") == "true")
            {
                
            if (id == null)
            {
                return NotFound();
            }

            var registo = await _context.Registos
                .Include(r => r.Cliente)
                .Include(r => r.Funcionario)
                .Include(r => r.Quarto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registo == null)
            {
                return NotFound();
            }

            return View(registo);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: Registo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registo = await _context.Registos.FindAsync(id);
            if (registo != null)
            {
                _context.Registos.Remove(registo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> ExportCsv()
        {
            if(HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" || HttpContext.Session.GetString("LOGADO") == "true")
            {
                var registos = await _context.Registos.Include(r => r.Cliente)
                                                        .Include(r => r.Funcionario)
                                                        .Include(r => r.Quarto)
                                                        .ToListAsync();

                // Criar o CSV 
                var builder = new StringBuilder();
                builder.AppendLine("Id,CheckIn,CheckOut,Quarto,Cliente,Funcionario,Total Dias Estadia");

                foreach (var registo in registos)
                {
                    builder.AppendLine($"{registo.Id},{registo.CheckIn},{registo.CheckOut},{registo.Quarto.TipoQuarto},{registo.Cliente.Nome},{registo.Funcionario.Nome},{registo.TotalDiasEstadia}");
                }

                // Return CSV num download com diretorio estatico
                var csvContent = builder.ToString();
                var csvBytes = Encoding.UTF8.GetBytes(csvContent);

                return File(csvBytes, "text/csv", "Reservas.csv");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        public async Task<IActionResult> ExportToPdf()
        {
            if(HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" || HttpContext.Session.GetString("LOGADO") == "true")
            {
                var registos = await _context.Registos.Include(r => r.Cliente)
                                                        .Include(r => r.Funcionario)
                                                        .Include(r => r.Quarto)
                                                        .ToListAsync();

                // Create PDF
                var pdfStream = new MemoryStream();
                var writer = new PdfWriter(pdfStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);
                    

                document.SetMargins(40, 40, 40, 40);
                document.Add(new Paragraph("Relatório de Reservas").SetFontSize(20).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph("Este relatório contém detalhes das reservas.").SetFontSize(12).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph("\n")); // Espaço entre o título e a tabela

                var table = new Table(7);

                table.AddHeaderCell("Id");
                table.AddHeaderCell("CheckIn");
                table.AddHeaderCell("CheckOut");
                table.AddHeaderCell("Quarto");
                table.AddHeaderCell("Cliente");
                table.AddHeaderCell("Funcionario");
                table.AddHeaderCell("Total Dias Estadia");

                foreach (var registo in registos)
                {
                    table.AddCell(registo.Id.ToString());
                    table.AddCell(registo.CheckIn.ToString());
                    table.AddCell(registo.CheckOut.ToString());
                    table.AddCell(registo.Quarto.TipoQuarto.ToString());
                    table.AddCell(registo.Cliente.Nome);
                    table.AddCell(registo.Funcionario.Nome);
                    table.AddCell(registo.TotalDiasEstadia.ToString());
                }

                document.Add(table);
                document.Close();

                // Return PDF como ficheiro para download
                var pdfBytes = pdfStream.ToArray();
                return File(pdfBytes, "application/pdf", "Reservas.pdf");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        private bool RegistoExists(int id)
        {
            return _context.Registos.Any(e => e.Id == id);
        }
    }
}
