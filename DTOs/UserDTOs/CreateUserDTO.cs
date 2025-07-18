namespace GoDecola.API.DTOs.UserDTOs
{
    public class CreateUserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Document { get; set; } // passport or cpf
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
