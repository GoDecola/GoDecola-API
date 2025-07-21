namespace GoDecola.API.DTOs.TravelPackageDTOs
{
    public class UpdateTravelPackageDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberGuests { get; set; }
        public int NumberBaths { get; set; }
        public int NumberBeds { get; set; }
        public HotelAmenitiesDTO Amenities { get; set; } = new HotelAmenitiesDTO();
        public string? Location { get; set; }
    }
}