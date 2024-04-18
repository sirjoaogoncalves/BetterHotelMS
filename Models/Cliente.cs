namespace GestaoHotelJoao.Models;

using System.ComponentModel.DataAnnotations;

public partial class Cliente
{
    public int Id { get; set; }
    
    [Required (ErrorMessage = "O nome é obrigatório")]
    public string? Nome { get; set; } = string.Empty;
    
    [Required (ErrorMessage = "O contacto é obrigatório")]
    public int Contacto { get; set; }

    public ICollection<Registo> Registos { get; set; } = new List<Registo>();
}
