using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgenciaViagens.Domain.Entities
{

    public class PaymentMethod
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
    }

}
