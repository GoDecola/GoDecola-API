using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoDecola.Domain.Entities
{

    public class Discount
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string Description { get; set; } = null!;
        public int Percentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
