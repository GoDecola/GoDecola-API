using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgenciaViagens.Domain.Entities
{

    public class TravelPackage
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public string? Duration { get; set; }
        public decimal BasePrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }

}
