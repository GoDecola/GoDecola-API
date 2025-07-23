using GoDecola.API.Entities;

namespace GoDecola.API.DTOs.ReservationDTOs
{
    public class ReservationDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TravelPackageId { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public string? Status { get; set; } = "Pending"; // "Pending", "Confirmed", "Cancelled"
        public double TotalPrice { get; set; }
        public User? User { get; set; } // propriedade de navegação para o usuário
        public TravelPackage? TravelPackage { get; set; } // propriedade de navegação para o pacote de viagem
        public ICollection<Guests>? Guests { get; set; } // lista de hospedes associados à reserva
    }
}
