namespace GoDecola.API.DTOs
{
    public class AddressDTO
    {
        public int Id { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Neighborhood { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
