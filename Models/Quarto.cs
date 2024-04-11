using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestaoHotelJoao.Models;


public partial class Quarto
{
    public int Id { get; set; }
    
    [DisplayName("Tipo de Quarto")]
    public string TipoQuarto { get; set; } = string.Empty;

    [DisplayName("Custo por Noite")]
    public decimal CustoNoite { get; set; }

    public ICollection<Registo> Registos { get; set; } = new List<Registo>();
}
