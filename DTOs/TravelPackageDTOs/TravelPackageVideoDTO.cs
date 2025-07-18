using GoDecola.API.Entities;

namespace GoDecola.API.DTOs.TravelPackageDTOs
{
    public class TravelPackageVideoDTO
    {
        public int Id { get; set; }
        public int TravelPackageId { get; set; } // fk
        public string? VideoUrl { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public TravelPackage? TravelPackage { get; set; }
    }
}
