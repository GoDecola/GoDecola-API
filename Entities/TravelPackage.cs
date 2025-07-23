using GoDecola.API.Enums;

namespace GoDecola.API.Entities
{
    public class TravelPackage
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberGuests { get; set; }
        public AccommodationDetails AccommodationDetails { get; set; } = new AccommodationDetails();
        public ICollection<TravelPackageMedia> Medias { get; set; } = new List<TravelPackageMedia>();
        public PackageType PackageType { get; set; }

        public double? DiscountPercentage { get; set; } // ex: 0.10 para 10% de desconto
        public DateTime? PromotionStartDate { get; set; }
        public DateTime? PromotionEndDate { get; set; }
    }
}
