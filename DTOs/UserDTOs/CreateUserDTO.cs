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
        public string? CPF { get; set; }
        public string? RNE { get; set; }
        public string? Passaport { get; set; }

    }
}
