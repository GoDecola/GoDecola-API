namespace GoDecola.API.Entities
{
    public class AccommodationDetails
    {
        public int Id { get; set; }
        public int TravelPackageId { get; set; }
        public int NumberBaths { get; set; }
        public int NumberBeds { get; set; }
        public bool HasWifi { get; set; }
        public bool HasParking { get; set; }
        public bool HasPool { get; set; }
        public bool HasGym { get; set; }
        public bool HasRestaurant { get; set; }
        public bool HasPetFriendly { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasBreakfastIncluded { get; set; }
        public Address? Address { get; set; } // propriedade de navegação para o endereço
    }
}
