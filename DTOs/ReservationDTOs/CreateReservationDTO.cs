using GoDecola.API.DTOs.TravelPackageDTOs;
using GoDecola.API.DTOs.UserDTOs;
using GoDecola.API.Entities;

namespace GoDecola.API.DTOs.ReservationDTOs
{
    public class CreateReservationDTO
    {
        public string UserId { get; set; }
        public int TravelPackageId { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public string? Status { get; set; } = "Pending"; // "Pending", "Confirmed", "Cancelled"
        public double TotalPrice { get; set; }
        public UserDTO? User { get; set; } // propriedade de navegação para o usuário
        public TravelPackageDTO? TravelPackage { get; set; } // propriedade de navegação para o pacote de viagem
        public ICollection<GuestsDTO>? Guests { get; set; } // lista de hospedes associados à reserva
    }
}
