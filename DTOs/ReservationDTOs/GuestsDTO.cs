namespace GoDecola.API.DTOs.ReservationDTOs
{
    public class GuestsDTO
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? CPF { get; set; }
        public string? RNE { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
