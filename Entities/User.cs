using Microsoft.AspNetCore.Identity;

namespace GoDecola.API.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Document { get; set; }

    }
}
