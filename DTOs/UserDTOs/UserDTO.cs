namespace GoDecola.API.DTOs.UserDTOs
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Document { get; set; } // pode ser cpf ou rne
        public string? Passaport { get; set; }
        

    }
}
