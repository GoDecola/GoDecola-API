using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoDecola.Domain.Entities
{

    public class PackageMedia
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public TravelPackage? Package { get; set; }

        public string MediaType { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string Url { get; set; } = null!;
    }

}
