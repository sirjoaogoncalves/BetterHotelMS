using System.ComponentModel.DataAnnotations;
namespace GestaoHotelJoao.Models;

public partial class User
{
    public int Id { get; set; }

    [Required (ErrorMessage = "O campo {0} é obrigatório")]
    public string? Username { get; set; } = string.Empty;

    [Required (ErrorMessage = "O campo {0} é obrigatório")]
    public string? Senha { get; set; } = string.Empty;

    public bool Administrador { get; set; }
 
    public bool Funcionario { get; set; }
}
