using System.ComponentModel.DataAnnotations;

namespace GoDecola.API.DTOs.ReviewDTOs
{
    public class CreateReviewDTO
    {
        public int TravelPackageId { get; set; }
        [MaxLength(500, ErrorMessage = "Limite maximo de 500 caracteres.")]
        public string Comment { get; set; } = string.Empty;
        [Range(0, 5, ErrorMessage = "Avaliação deve ser de 0 a 5")]
        public int Rating { get; set; }  
        public DateTime CreatedAt { get; set; }
    }
}
