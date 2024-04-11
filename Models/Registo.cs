using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GestaoHotelJoao.Models;

public partial class Registo
{
    public int Id { get; set; }
    
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly CheckIn { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly CheckOut { get; set; }

    [DisplayName("Tipo de Quarto")]
    public int QuartoId { get; set; }

    [DisplayName("Cliente")]
    public int ClienteId { get; set; }

    [DisplayName("Funcionário")]
    public int FuncionarioId { get; set; }
    
    [DisplayName("Total de Dias")]
    [DisplayFormat(DataFormatString = "{0:0}")]
    public decimal TotalDiasEstadia { get; set; }

    public Cliente? Cliente { get; set; }

    public Funcionario? Funcionario { get; set; } 

    public Quarto? Quarto { get; set; } 
}
