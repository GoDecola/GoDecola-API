namespace GoDecola.API.DTOs.UserDTOs
{
    public class LoginResponseDTO
    {
        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
