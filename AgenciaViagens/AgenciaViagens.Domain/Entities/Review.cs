using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgenciaViagens.Domain.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public TravelPackage? Package { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; } = false;
    }

}
