namespace GoDecola.API.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public User User { get; set; }
        public int TravelPackageId { get; set; }
        public TravelPackage TravelPackage { get; set; }
        public double Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
