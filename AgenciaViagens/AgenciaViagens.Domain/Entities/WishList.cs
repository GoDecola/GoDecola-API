using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgenciaViagens.Domain.Entities
{

    public class WishList
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid PackageId { get; set; }
        public TravelPackage? Package { get; set; }
    }

}
