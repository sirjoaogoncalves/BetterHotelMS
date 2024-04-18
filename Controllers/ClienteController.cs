using Microsoft.AspNetCore.Mvc;
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
    public class ClienteController : Controller
    {
        private readonly DbGestaoHotelJoaoContext _context;

        public ClienteController(DbGestaoHotelJoaoContext context)
        {
            _context = context;
        }

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("ADMIN") == "true" || 
                    (HttpContext.Session.GetString("FUNCIONARIO") == "true" && HttpContext.Session.GetString("LOGADO") == "true"))
            {
                return View(await _context.Clientes.ToListAsync());
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            if ((HttpContext.Session.GetString("ADMIN") == "true" ||
                        HttpContext.Session.GetString("FUNCIONARIO") == "true") && 
                    HttpContext.Session.GetString("LOGADO") == "true")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Contacto")] Cliente cliente)
        {
            if ((HttpContext.Session.GetString("ADMIN") == "true" ||
                        HttpContext.Session.GetString("FUNCIONARIO") == "true") && 
                    HttpContext.Session.GetString("LOGADO") == "true")
            {
                if (ModelState.IsValid)
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(cliente);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if ((HttpContext.Session.GetString("ADMIN") == "true" ||
                        HttpContext.Session.GetString("FUNCIONARIO") == "true") && 
                    HttpContext.Session.GetString("LOGADO") == "true")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: Cliente/Edit/5 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Contacto")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if ((HttpContext.Session.GetString("ADMIN") == "true" ||
                        HttpContext.Session.GetString("FUNCIONARIO") == "true") && 
                    HttpContext.Session.GetString("LOGADO") == "true")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportCsv()
        {
            if(HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" && HttpContext.Session.GetString("LOGADO") == "true")
            {
                var clientes = await _context.Clientes.ToListAsync(); 
                // CSV 
                var builder = new StringBuilder();
                builder.AppendLine("Id,Nome,Contacto");

                foreach (var cliente in clientes)
                {

                    builder.AppendLine($"{cliente.Id},{cliente.Nome},{cliente.Contacto}");
                }

                // Return CSV num download com diretorio estatico
                var csvContent = builder.ToString();
                var csvBytes = Encoding.UTF8.GetBytes(csvContent);

                return File(csvBytes, "text/csv", "Clientes.csv");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public async Task<IActionResult> ExportPdf()
        {

            if (HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" && HttpContext.Session.GetString("LOGADO") == "true")
            {
                var clientes = await _context.Clientes.ToListAsync();

                var pdfStream = new MemoryStream();
                var writer = new PdfWriter(pdfStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                document.SetMargins(40, 40, 40, 40);
                document.Add(new Paragraph("Relatório de Clientes").SetFontSize(20).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph("Este relatório contém detalhes dos clientes.").SetFontSize(12).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph("\n")); // Espaço entre o título e a tabela                

                Div div = new Div();
                div.SetTextAlignment(TextAlignment.CENTER);
                div.Add(new Paragraph("")); 
                var table = new Table(3);


                table.AddHeaderCell("Id");
                table.AddHeaderCell("Nome");
                table.AddHeaderCell("Contacto");


                foreach (var cliente in clientes)
                {

                    table.AddCell(cliente.Id.ToString());
                    table.AddCell(cliente.Nome);
                    table.AddCell(cliente.Contacto.ToString());
                }

                document.Add(table);
                document.Close();

                // Return PDF como ficheiro para download
                var pdfBytes = pdfStream.ToArray();
                return File(pdfBytes, "application/pdf", "Clientes.pdf");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public async Task<IActionResult> ImportCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file to import.");
                return View();
            }

            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Please select a CSV file.");
                return View();
            }

            // Read the CSV file
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                List<Cliente> clientes = new List<Cliente>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Skip the header line
                    if (line.StartsWith("Id,Nome,Contacto"))
                        continue;

                    var values = line.Split(',');
                    if (values.Length != 3)
                    {
                        ModelState.AddModelError("", "Invalid CSV format.");
                        return View();
                    }

                    var cliente = new Cliente
                    {
                        Nome = values[1],
                        Contacto = Convert.ToInt32(values[2])
                    };

                    clientes.Add(cliente);
                }

                // Add clients to the database
                await _context.AddRangeAsync(clientes);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}

