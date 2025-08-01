using GoDecola.API.Enums;
using Microsoft.AspNetCore.Identity;

namespace GoDecola.API.Entities
{
    public class User : IdentityUser
    {

        // id, email, senha
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? CPF { get; set; }
        public string? RNE { get; set; }
        public string? Passaport { get; set; }
        ICollection<Reservation>? Reservations { get; set; } // lista de reservas associadas ao usuário
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // data de criação do usuário
    }
}
