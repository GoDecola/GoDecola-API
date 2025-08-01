using GoDecola.API.Enums;

namespace GoDecola.API.DTOs.UserDTOs
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Document { get; set; } // pode ser cpf ou rne
        public string? Passaport { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
