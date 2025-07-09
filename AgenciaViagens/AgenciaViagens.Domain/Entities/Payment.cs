using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgenciaViagens.Domain.Entities
{

    public class Payment
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public Guid PaymentMethodId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "pending";

        public Guid? DiscountAppliedId { get; set; }
        public Discount? DiscountApplied { get; set; }
    }

}
