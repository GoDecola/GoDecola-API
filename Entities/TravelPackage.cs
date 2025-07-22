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
        public int NumberBaths { get; set; }
        public int NumberBeds { get; set; }
        public HotelAmenities Amenities { get; set; } = new HotelAmenities();
        public string? Location { get; set; }
        public ICollection<TravelPackageMedia> Medias { get; set; } = new List<TravelPackageMedia>();
    }
}
