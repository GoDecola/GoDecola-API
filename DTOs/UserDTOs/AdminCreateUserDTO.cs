namespace GoDecola.API.DTOs.UserDTOs
{
    public class AdminCreateUserDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Document { get; set; }
        public required string Role { get; set; }
    }
}
