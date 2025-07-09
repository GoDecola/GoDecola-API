using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgenciaViagens.Domain.Entities
{

    public class UserType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<User>? Users { get; set; }
    }

}
