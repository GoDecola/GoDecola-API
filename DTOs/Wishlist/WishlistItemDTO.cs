namespace GoDecola.API.DTOs.Wishlist
{
    public class WishlistItemDTO
    {
        public int TravelPackageId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; } // baseado no status do pacote
        public DateTime AddedDate { get; set; }
    }
}
