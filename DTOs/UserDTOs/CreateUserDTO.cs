using GoDecola.API.Enums;

namespace GoDecola.API.DTOs.UserDTOs
{
    public class CreateUserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? CPF { get; set; }
        public string? RNE { get; set; }
        public string? Passaport { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
