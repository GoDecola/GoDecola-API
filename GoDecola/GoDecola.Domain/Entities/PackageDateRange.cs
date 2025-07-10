using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoDecola.Domain.Entities
{

    public class PackageDateRange
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public TravelPackage? Package { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

}
