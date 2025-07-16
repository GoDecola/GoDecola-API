namespace GoDecola.API.Entities
{
    public class TravelPackage
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberGuests { get; set; }
        public int NumberBaths { get; set; }
        public int NumberBeds { get; set; }
        public HotelAmenities Amenities { get; set; } = new HotelAmenities();

        //public ICollection<ItemPedido>? Itens { get; set; } = new List<ItemPedido>();

    }
}
