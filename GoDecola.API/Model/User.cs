using GoDecola.API.ValidationAttribute;
using System.ComponentModel.DataAnnotations;

namespace GoDecola.API.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public UserType Tipo { get; set; } = UserType.Cliente;

    }
}
