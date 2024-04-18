namespace GestaoHotelJoao.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string? Nome { get; set; } = string.Empty;

    public int Contacto { get; set; }

    public ICollection<Registo> Registos { get; set; } = new List<Registo>();
}
