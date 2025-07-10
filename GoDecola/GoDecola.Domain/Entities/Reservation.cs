using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoDecola.Domain.Entities
{

    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public TravelPackage? Package { get; set; }

        public Guid PackageDateRangeId { get; set; }
        public PackageDateRange? PackageDateRange { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public int NumTravelers { get; set; }
        public string Status { get; set; } = "pending";
        public decimal TotalAmount { get; set; }
        public string ReservationCode { get; set; } = null!;
    }

}
