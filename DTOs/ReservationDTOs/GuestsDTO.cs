namespace GoDecola.API.DTOs.ReservationDTOs
{
    public class GuestsDTO
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Document { get; set; } // passport or cpf
        public DateTime DateOfBirth { get; set; }
    }
}
