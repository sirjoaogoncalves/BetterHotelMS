namespace GestaoHotelJoao.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; } = string.Empty;

    public string? Senha { get; set; } = string.Empty;

    public bool Administrador { get; set; }

    public bool Funcionario { get; set; }
}
