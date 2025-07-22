using GoDecola.API.Enums;

namespace GoDecola.API.Entities
{
    public class TravelPackageMedia
    {
        public int Id { get; set; }
        public int TravelPackageId { get; set; }
        public string? MediaUrl { get; set; }
        public MediaType MediaType { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public TravelPackage? TravelPackage { get; set; } // propriedade de navegação
    }
}
