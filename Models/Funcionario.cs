using System.ComponentModel;

namespace GestaoHotelJoao.Models;

public partial class Funcionario
{
    public int Id { get; set; }
    
    [DisplayName("Nome Funcionário")]
    public string? Nome { get; set; } = string.Empty;

    public ICollection<Registo> Registos { get; set; } = new List<Registo>();
}
