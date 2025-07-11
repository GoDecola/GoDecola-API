﻿using System.ComponentModel.DataAnnotations;

namespace GoDecola.API.DTO.Request
{
    public record LoginRequestDTO(
        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        string Email,

        [Required(ErrorMessage = "Senha é obrigatória.")]
        string Password
    );
}
