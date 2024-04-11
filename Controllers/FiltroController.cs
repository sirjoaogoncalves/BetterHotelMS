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

        // GET: Registo
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" || HttpContext.Session.GetString("LOGADO") == "true")
            {
                var registos = await _context.Registos
                 .Include(r => r.Cliente)
                 .Include(r => r.Funcionario)
                 .ToListAsync();
                return View(registos);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // Filtragem de registos por data
        public async Task<IActionResult> FiltrarPorData(DateOnly dataInicio, DateOnly dataFim)
        {
            var registosHoje = await _context.Registos
                .Include(r => r.Cliente)
                .Include(r => r.Funcionario)
                .Where(r => r.CheckIn >= dataInicio && r.CheckOut <= dataFim)
                .ToListAsync();
            return View("Index", registosHoje);
        }


        // Filtragem de registos por cliente
        public async Task<IActionResult> FiltrarPorCliente(string clienteName)
        {
            var registosCliente = await _context.Registos
                .Include(r => r.Cliente)
                .Include(r => r.Funcionario)
                .Where(r => r.Cliente.Nome.Contains(clienteName))
                .ToListAsync();

            return View("Index", registosCliente);
        }

        // Filtragem de registos por funcionário
        public async Task<IActionResult> FiltrarPorFuncionario(string funcionarioName)
        {
            var registosFuncionario = await _context.Registos
                .Include(r => r.Cliente)
                .Include(r => r.Funcionario)
                .Where(r => r.Funcionario.Nome.Contains(funcionarioName))
                .ToListAsync();

            return View("Index", registosFuncionario);
        }

        public async Task<IActionResult> FiltrarPorTipo(string tipoFiltro, string funcionarioName, string tipoQuarto, DateOnly dataInicio, DateOnly dataFim)
        {
            decimal totalGain = 0;

            if (tipoFiltro == "Funcionario")
            {
                var ganhoTotalPorFuncionario = await _context.Registos
                    .Include(r => r.Quarto)
                    .Include(r => r.Funcionario)
                    .Include(r => r.Cliente)
                    .Where(r => r.Funcionario.Nome == funcionarioName && r.CheckIn >= dataInicio && r.CheckOut <= dataFim)
                    .ToListAsync();

                totalGain = ganhoTotalPorFuncionario.Sum(r => r.Quarto.CustoNoite * r.TotalDiasEstadia);

                ViewBag.TotalGain = totalGain;

                return View("Index", ganhoTotalPorFuncionario);
            }
            else if (tipoFiltro == "Quarto")
            {
                var ganhoTotalPorQuarto = await _context.Registos
                    .Include(r => r.Quarto)
                    .Include(r => r.Funcionario)
                    .Include(r => r.Cliente)
                    .Where(r => r.Quarto.TipoQuarto == tipoQuarto && r.CheckIn >= dataInicio && r.CheckOut <= dataFim)
                    .ToListAsync();

                totalGain = ganhoTotalPorQuarto.Sum(r => r.Quarto.CustoNoite * r.TotalDiasEstadia);

                ViewBag.TotalGain = totalGain;

                return View("Index", ganhoTotalPorQuarto);
            }

            return View("Index");
        }

        public async Task<IActionResult> ExportCsv()
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" || HttpContext.Session.GetString("LOGADO") == "true")
            {
                var registos = await _context.Registos.Include(r => r.Cliente)
                                                        .Include(r => r.Funcionario)
                                                        .Include(r => r.Quarto)
                                                        .ToListAsync();
                // CSV 
                var builder = new StringBuilder();
                builder.AppendLine("Id,CheckIn,CheckOut,Quarto,Cliente,Funcionario,Total Dias Estadia,Ganho Total Estadia");

                foreach (var registo in registos)
                {
                    var ganhoReserva = registo.Quarto.CustoNoite * registo.TotalDiasEstadia;

                    builder.AppendLine($"{registo.Id},{registo.CheckIn},{registo.CheckOut},{registo.Quarto.TipoQuarto},{registo.Cliente.Nome},{registo.Funcionario.Nome},{registo.TotalDiasEstadia},{ganhoReserva}");
                }

                // Return CSV num download com diretorio estatico
                var csvContent = builder.ToString();
                var csvBytes = Encoding.UTF8.GetBytes(csvContent);

                return File(csvBytes, "text/csv", "RegistosFiltragem.csv");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public async Task<IActionResult> ExportToPdf()
        {
            if (HttpContext.Session.GetString("ADMIN") == "true" || HttpContext.Session.GetString("FUNCIONARIO") == "true" || HttpContext.Session.GetString("LOGADO") == "true")
            {
                var registos = await _context.Registos.Include(r => r.Cliente)
                                                        .Include(r => r.Funcionario)
                                                        .Include(r => r.Quarto)
                                                        .ToListAsync();

                var ganhosTotais = registos.Sum(r => r.Quarto.CustoNoite * r.TotalDiasEstadia);

                // Criar o PDF
                var pdfStream = new MemoryStream();
                var writer = new PdfWriter(pdfStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);
                
                
                document.SetMargins(40, 40, 40, 40);
                document.Add(new Paragraph("Relatório de Reservas").SetFontSize(20).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph("Este relatório contém detalhes das reservas filtradas.").SetFontSize(12).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph("\n")); // Espaço entre o título e a tabela                

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

                // Return PDF como ficheiro para download
                var pdfBytes = pdfStream.ToArray();
                return File(pdfBytes, "application/pdf", "ReservasFiltradas.pdf");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


    }
}
