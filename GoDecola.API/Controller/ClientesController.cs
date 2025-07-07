using GoDecola.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace GoDecola.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private static readonly List<Clientes> clientes = new();

        [HttpPost(Name = "/cadastro")]
        public ActionResult CadastrarCliente([FromBody] Clientes novoCliente)
        {
            novoCliente.Id = Guid.NewGuid();
            clientes.Add(novoCliente);

            return CreatedAtAction(null, new { id = novoCliente.Id }, null);
        }
    }
}
