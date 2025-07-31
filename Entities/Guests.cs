namespace GoDecola.API.Entities
{
    public class Guests
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? CPF { get; set; }
        public string? RNE { get; set; }
        public string? Passaport { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
