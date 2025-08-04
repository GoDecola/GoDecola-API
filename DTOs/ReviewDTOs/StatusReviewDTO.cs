using GoDecola.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace GoDecola.API.DTOs.ReviewDTOs
{
    public class StatusReviewDTO
    {
        [Required]
        [EnumDataType(typeof(ReviewStatus))]
        public ReviewStatus Status { get; set; }
    }
}
