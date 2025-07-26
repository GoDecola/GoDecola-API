using GoDecola.API.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoDecola.API.Entities
{
    public class TravelPackage
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public long Price { get; set; }
        public string? Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int NumberGuests { get; set; }
        public AccommodationDetails AccommodationDetails { get; set; } = new AccommodationDetails();
        public ICollection<TravelPackageMedia> Medias { get; set; } = new List<TravelPackageMedia>();
        public PackageType PackageType { get; set; }
        public double? DiscountPercentage { get; set; } // ex: 0.10 para 10% de desconto
        public DateTime? PromotionStartDate { get; set; }
        public DateTime? PromotionEndDate { get; set; }

        [NotMapped]
        public bool IsCurrentlyOnPromotion
        {
            get
            {
                // se a varaivel DiscountPercentage nao estiver definida ou for menor ou igual a zero, nao esta em promo
                if (!DiscountPercentage.HasValue || DiscountPercentage.Value <= 0)
                {
                    return false;
                }

                DateTime now = DateTime.UtcNow;

                // se as datas de promocao nao estao definidas ou é maior que a data atual (invalido), nao esta em promocao
                if (PromotionStartDate.HasValue && PromotionStartDate.Value > now)
                {
                    return false;
                }

                // se a data de promocao final estiver definida e for menor que a data atual (invalido), nao esta em promocao
                if (PromotionEndDate.HasValue && PromotionEndDate.Value < now)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
