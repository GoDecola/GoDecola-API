namespace GoDecola.API.DTOs.TravelPackageDTOs
{
    public class TravelPackageFilterDto
    {
        public string? Destination { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? NumberGuests { get; set; }
        public long? PriceMin { get; set; }
        public long? PriceMax { get; set; }
    }


}
