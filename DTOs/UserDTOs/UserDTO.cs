namespace GoDecola.API.DTOs.UserDTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Document { get; set; } // passport or cpf
        public string? Email { get; set; }
    }
}
