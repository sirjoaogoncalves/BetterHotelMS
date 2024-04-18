using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestaoHotelJoao.Models;


public partial class Quarto
{
    public int Id { get; set; }
    
    [DisplayName("Tipo de Quarto")]
    [Required(ErrorMessage = "O tipo de quarto é obrigatório")]
    public string? TipoQuarto { get; set; } = string.Empty;

    [DisplayName("Custo por Noite")]
    [Required(ErrorMessage = "O custo por noite é obrigatório")]
    public decimal CustoNoite { get; set; }

    public ICollection<Registo> Registos { get; set; } = new List<Registo>();
}
