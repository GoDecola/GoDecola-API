using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoDecola.Domain.Entities
{

    public class User
    {
        public Guid Id { get; set; }
        public Guid UserTypeId { get; set; }
        public UserType? UserType { get; set; }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Phone { get; set; }
        public string? CPF { get; set; }
        public string? Passport { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
