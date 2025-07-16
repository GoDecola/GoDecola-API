namespace GoDecola.API.Entities
{
    public class Guests
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Document { get; set; } // passport or cpf
        public DateTime DateOfBirth { get; set; }
    }
}
