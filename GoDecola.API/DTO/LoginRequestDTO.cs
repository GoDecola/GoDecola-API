using System.ComponentModel.DataAnnotations;

namespace GoDecola.API.DTO
{
    public record LoginRequestDTO(
        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        string Email,

        [Required(ErrorMessage = "Senha é obrigatória.")]
        string Password
    );
}
