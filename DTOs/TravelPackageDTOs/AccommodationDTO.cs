namespace GoDecola.API.DTOs.TravelPackageDTOs
{
    public class AccommodationDTO
    {
        public int Id { get; set; }
        public int NumberBaths { get; set; }
        public int NumberBeds { get; set; }
        public bool HasWifi { get; set; } = false;
        public bool HasParking { get; set; } = false;
        public bool HasPool { get; set; } = false;
        public bool HasGym { get; set; } = false;
        public bool HasRestaurant { get; set; } = false;
        public bool HasPetFriendly { get; set; } = false;
        public bool HasAirConditioning { get; set; } = false;
        public bool HasBreakfastIncluded { get; set; } = false;
        public AddressDTO? Address { get; set; } // propriedade de navegação para o endereço
    }
}