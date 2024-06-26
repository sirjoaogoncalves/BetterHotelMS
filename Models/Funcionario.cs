﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestaoHotelJoao.Models;

public partial class Funcionario
{
    public int Id { get; set; }
    
    [DisplayName("Nome Funcionário")]
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string? Nome { get; set; } = string.Empty;

    public ICollection<Registo> Registos { get; set; } = new List<Registo>();
}
