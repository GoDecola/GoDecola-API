using GoDecola.API.Enums;

namespace GoDecola.API.DTOs.TravelPackageDTOs
{
    public class TravelPackageMediaDTO
    {
        public int Id { get; set; }
        public int TravelPackageId { get; set; } // fk
        public string? MediaUrl { get; set; }
        public MediaType MediaType { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }
}
