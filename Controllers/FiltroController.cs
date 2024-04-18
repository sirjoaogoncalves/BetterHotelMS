using GestaoHotelJoao.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace GestaoHotelJoao.Controllers
{
    public class FiltroController : Controller
    {
        private readonly DbGestaoHotelJoaoContext _context;

        public FiltroController(DbGestaoHotelJoaoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!UserLoggedIn())
                return RedirectToAction("Index", "Login");

            var registos = await _context.Registos
                .Include(r => r.Cliente)
                .Include(r => r.Funcionario)
                .ToListAsync();

            return View(registos);
        }

        public async Task<IActionResult> FiltrarPorData(DateOnly dataInicio, DateOnly dataFim)
        {
            if (!UserLoggedIn())
                return RedirectToAction("Index", "Login");

            var registos = await _context.Registos
                .Include(r => r.Cliente)
                .Include(r => r.Funcionario)
                .Where(r => r.CheckIn >= dataInicio && r.CheckOut <= dataFim)
                .ToListAsync();

            return View("Index", registos);
        }

        public async Task<IActionResult> FiltrarPorCliente(string clienteName)
        {
            if (!UserLoggedIn())
                return RedirectToAction("Index", "Login");

            var registos = await _context.Registos
                .Include(r => r.Cliente)
                .Include(r => r.Funcionario)
                .Where(r => r.Cliente.Nome.Contains(clienteName))
                .ToListAsync();

            return View("Index", registos);
        }

        public async Task<IActionResult> FiltrarPorFuncionario(string funcionarioName)
        {
            if (!UserLoggedIn())
                return RedirectToAction("Index", "Login");

            var registos = await _context.Registos
                .Include(r => r.Cliente)
                .Include(r => r.Funcionario)
                .Where(r => r.Funcionario.Nome.Contains(funcionarioName))
                .ToListAsync();

            return View("Index", registos);
        }

        public async Task<IActionResult> FiltrarPorTipo(string tipoFiltro, string funcionarioName, string tipoQuarto, DateOnly dataInicio, DateOnly dataFim)
        {
            if (!UserLoggedIn())
                return RedirectToAction("Index", "Login");

            var query = _context.Registos
                .Include(r => r.Quarto)
                .Include(r => r.Funcionario)
                .Include(r => r.Cliente)
                .Where(r => r.CheckIn >= dataInicio && r.CheckOut <= dataFim);

            if (tipoFiltro == "Funcionario")
                query = query.Where(r => r.Funcionario.Nome == funcionarioName);
            else if (tipoFiltro == "Quarto")
                query = query.Where(r => r.Quarto.TipoQuarto == tipoQuarto);

            var registos = await query.ToListAsync();
            ViewBag.TotalGain = registos.Sum(r => r.Quarto.CustoNoite * r.TotalDiasEstadia);

            return View("Index", registos);
        }

        public async Task<IActionResult> ExportCsv()
        {
            if (!UserLoggedIn())
                return RedirectToAction("Index", "Login");

            var registos = await _context.Registos
                .Include(r => r.Cliente)
                .Include(r => r.Funcionario)
                .Include(r => r.Quarto)
                .ToListAsync();

            var csvContent = new StringBuilder();
            csvContent.AppendLine("Id,CheckIn,CheckOut,Quarto,Cliente,Funcionario,Total Dias Estadia,Ganho Total Estadia");

            foreach (var registo in registos)
            {
                var ganhoReserva = registo.Quarto.CustoNoite * registo.TotalDiasEstadia;
                csvContent.AppendLine($"{registo.Id},{registo.CheckIn},{registo.CheckOut},{registo.Quarto.TipoQuarto},{registo.Cliente.Nome},{registo.Funcionario.Nome},{registo.TotalDiasEstadia},{ganhoReserva}");
            }

            return File(Encoding.UTF8.GetBytes(csvContent.ToString()), "text/csv", "RegistosFiltragem.csv");
        }

        public async Task<IActionResult> ExportToPdf()
        {
            if (!UserLoggedIn())
                return RedirectToAction("Index", "Login");

            var registos = await _context.Registos
                .Include(r => r.Cliente)
                .Include(r => r.Funcionario)
                .Include(r => r.Quarto)
                .ToListAsync();

            var ganhosTotais = registos.Sum(r => r.Quarto.CustoNoite * r.TotalDiasEstadia);

            var pdfStream = new MemoryStream();
            var writer = new PdfWriter(pdfStream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            document.SetMargins(40, 40, 40, 40);
            document.Add(new Paragraph("Relatório de Reservas").SetFontSize(20).SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph("Este relatório contém detalhes das reservas filtradas.").SetFontSize(12).SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Reservas Filtrados"));

            var table = new Table(8);
            table.AddHeaderCell("Id");
            table.AddHeaderCell("CheckIn");
            table.AddHeaderCell("CheckOut");
            table.AddHeaderCell("Quarto");
            table.AddHeaderCell("Cliente");
            table.AddHeaderCell("Funcionario");
            table.AddHeaderCell("Total Dias Estadia");
            table.AddHeaderCell("Ganho Total Estadia");

            foreach (var registo in registos)
            {
                var ganhoReserva = registo.Quarto.CustoNoite * registo.TotalDiasEstadia;
                table.AddCell(registo.Id.ToString());
                table.AddCell(registo.CheckIn.ToString());
                table.AddCell(registo.CheckOut.ToString());
                table.AddCell(registo.Quarto.TipoQuarto.ToString());
                table.AddCell(registo.Cliente.Nome);
                table.AddCell(registo.Funcionario.Nome);
                table.AddCell(registo.TotalDiasEstadia.ToString());
                table.AddCell(ganhoReserva.ToString() + "€");
            }

            document.Add(table);
            document.Add(new Paragraph("Total Ganho: " + ganhosTotais + "€"));
            document.Close();

            return File(pdfStream.ToArray(), "application/pdf", "ReservasFiltradas.pdf");
        }

        private bool UserLoggedIn()
        {
            return HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" || HttpContext.Session.GetString("LOGADO") == "true";
        }
    }
}
