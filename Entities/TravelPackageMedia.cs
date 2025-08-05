using GoDecola.API.Enums;

namespace GoDecola.API.Entities
{
    public class TravelPackageMedia
    {
        public int Id { get; set; }
        public int TravelPackageId { get; set; }
        public string? FilePath { get; set; }
        public string? MimeType { get; internal set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public TravelPackage? TravelPackage { get; set; } // propriedade de navegação
    }
}
