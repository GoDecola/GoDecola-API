namespace GoDecola.API.Entities.TravelPackage
{
    public class TravelPackageVideo
    {
        public int Id { get; set; }
        public int TravelPackageId { get; set; } // fk
        public string? VideoUrl { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public TravelPackage? TravelPackage { get; set; }
    }
}
