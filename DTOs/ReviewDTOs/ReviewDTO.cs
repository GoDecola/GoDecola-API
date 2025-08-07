using GoDecola.API.Enums;

namespace GoDecola.API.DTOs.ReviewDTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public string? Comment { get; set; }
        public ReviewStatus Status { get; set; }
        public DateTime ReviewDate { get; set; }
        public ReviewUserDTO? User { get; set; }
        public bool IsEdited { get; set; }
    }
}
