using GoDecola.API.Entities;

namespace GoDecola.API.DTOs.TravelPackageDTOs
{
    public class TravelPackageImageDTO
    {
        public int Id { get; set; }
        public int TravelPackageId { get; set; } // fk
        public string? PhotoUrl { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }
}
