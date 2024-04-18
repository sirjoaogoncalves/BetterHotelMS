using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GestaoHotelJoao.Models;

public partial class Registo
{
    public int Id { get; set; }
    
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Required (ErrorMessage = "Data de Entrada obrigatória")]
    public DateOnly CheckIn { get; set; }

    [Required (ErrorMessage = "Data de Saída obrigatória")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly CheckOut { get; set; }

    [Required (ErrorMessage = "Tipo de Quarto obrigatório")]
    [DisplayName("Tipo de Quarto")]
    public int QuartoId { get; set; }

    [Required (ErrorMessage = "Cliente obrigatório")]
    [DisplayName("Cliente")]
    public int ClienteId { get; set; }

    [Required (ErrorMessage = "Funcionário obrigatório")]
    [DisplayName("Funcionário")]
    public int FuncionarioId { get; set; }
    
    [DisplayName("Total de Dias")]
    [DisplayFormat(DataFormatString = "{0:0}")]
    public decimal TotalDiasEstadia { get; set; }

    public Cliente? Cliente { get; set; }

    public Funcionario? Funcionario { get; set; } 

    public Quarto? Quarto { get; set; } 
}
