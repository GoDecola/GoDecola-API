namespace GoDecola.API.Entities
{
    public class HotelAmenities
    {
        public int Id { get; set; }
        public int TravelPackageId { get; set; }
        public TravelPackage TravelPackage { get; set; }
        public bool Wifi { get; set; }
        public bool Parking { get; set; }
        public bool Pool { get; set; }
        public bool Gym { get; set; }
        public bool Restaurant { get; set; }
        public bool PetFriendly { get; set; }
        public bool AirConditioning { get; set; }
        public bool BreakfastIncluded { get; set; }
    }
}
