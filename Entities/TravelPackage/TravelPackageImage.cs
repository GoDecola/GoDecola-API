namespace GoDecola.API.Entities.TravelPackage
{
    public class TravelPackageImage
    {
        public int Id { get; set; }
        public int TravelPackageId { get; set; } // fk
        public string? PhotoUrl { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public TravelPackage? TravelPackage { get; set; } // propriedade de navegação
    }
}
