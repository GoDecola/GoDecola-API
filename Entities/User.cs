using GoDecola.API.Enums;
using Microsoft.AspNetCore.Identity;

namespace GoDecola.API.Entities
{
    public class User : IdentityUser
    {

        // id, email, senha
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CPF { get; set; }
        public string? RNE { get; set; }
        public string? Passaport { get; set; }
        public UserType Type { get; set; } = UserType.USER; // Só pra garantir um valor default quando n for setado. E pq já temos um enum UserType
        ICollection<Reservation>? Reservations { get; set; } // lista de reservas associadas ao usuário
    }
}
