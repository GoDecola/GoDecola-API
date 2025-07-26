namespace GoDecola.API.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TravelPackageId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public long TotalPrice { get; set; }
        public string? Status { get; set; } = "Pending"; // "Pending", "Confirmed", "Cancelled"
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public User? User { get; set; } // propriedade de navegação para o usuário
        public TravelPackage? TravelPackage { get; set; } // propriedade de navegação para o pacote de viagem
        public ICollection<Guests>? Guests { get; set; } // lista de hospedes associados à reserva
    }
}
