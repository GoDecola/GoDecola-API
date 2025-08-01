using GoDecola.API.Entities;
using GoDecola.API.Enums;

namespace GoDecola.API.DTOs.TravelPackageDTOs
{
    public class TravelPackageDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public long Price { get; set; }
        public string? Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int NumberGuests { get; set; }
        public AccommodationDTO AccommodationDetails { get; set; } = new AccommodationDTO();
        public ICollection<TravelPackageMediaDTO> MediasUrl { get; set; } = new List<TravelPackageMediaDTO>();
        public PackageType PackageType { get; set; }
        public bool IsCurrentlyOnPromotion { get; set; }
        public double? DiscountPercentage { get; set; }
        public DateTime? PromotionStartDate { get; set; }
        public DateTime? PromotionEndDate { get; set; }
        public double? AverageRating { get; set; }
    }
}
