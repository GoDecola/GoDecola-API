using GoDecola.API.Enums;

namespace GoDecola.API.DTOs.TravelPackageDTOs
{
    public class TravelPackageMediaDTO
    {
        public int Id { get; set; }
        public int TravelPackageId { get; set; } // fk
        public string? FilePath { get; set; }
        public string? MimeType { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }
}
