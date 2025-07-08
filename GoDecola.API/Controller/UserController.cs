using GoDecola.API.DTO;
using GoDecola.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace GoDecola.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly List<User> users = new();

        [HttpPost(Name = "/cadastro")]
        public ActionResult CadastrarCliente([FromBody] User newUser)
        {
            newUser.Id = Guid.NewGuid();
            users.Add(newUser);

            return CreatedAtAction(null, new { id = newUser.Id }, null);
        }
    }
}
