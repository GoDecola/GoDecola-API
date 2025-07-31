using GoDecola.API.DTOs.TravelPackageDTOs;
using GoDecola.API.DTOs.UserDTOs;
using GoDecola.API.Entities;

namespace GoDecola.API.DTOs.ReservationDTOs
{
    public class CreateReservationDTO
    {
        public string? UserId { get; set; }
        public int TravelPackageId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public ICollection<GuestsDTO>? Guests { get; set; } // lista de hospedes associados à reserva
    }
}
