namespace GoDecola.API.DTOs.TravelPackageDTOs
{
    public class HotelAmenitiesDTO
    {
        public int Id { get; set; }
        public int TravelPackageId { get; set; }
        public bool HasWifi { get; set; }
        public bool HasParking { get; set; }
        public bool HasPool { get; set; }
        public bool HasGym { get; set; }
        public bool HasRestaurant { get; set; }
        public bool HasPetFriendly { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasBreakfastIncluded { get; set; }
    }
}