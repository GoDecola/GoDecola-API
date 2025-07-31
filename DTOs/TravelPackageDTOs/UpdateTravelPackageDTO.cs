using GoDecola.API.Enums;

namespace GoDecola.API.DTOs.TravelPackageDTOs
{
    public class UpdateTravelPackageDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public long? Price { get; set; }
        public string? Destination { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? NumberGuests { get; set; }
        public AccommodationDTO? AccommodationDetails { get; set; } = new AccommodationDTO();
        public bool? IsActive { get; set; }
        public double? DiscountPercentage { get; set; }
        public DateTime? PromotionStartDate { get; set; }
        public DateTime? PromotionEndDate { get; set; }
        public PackageType? PackageType { get; set; }
    }
}