using AutoMapper;
using GoDecola.API.DTO;
using GoDecola.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace GoDecola.API.Controller
{
    [ApiController]
    [Route("api/")]
    public class UserController : ControllerBase
    {
        private static readonly List<User> users = new();
        private readonly IMapper _mapper;

        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost("cadastro")]
        public ActionResult CadastrarCliente([FromBody] RegisterRequestDTO newUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = _mapper.Map<User>(newUserDTO);
            newUser.Id = Guid.NewGuid();
            newUser.Tipo = TipoUsuario.Cliente;

            users.Add(newUser);

            return CreatedAtAction(null, new { id = newUser.Id }, new {mensagem = "Cliente cadastrado com sucesso."});
        }

    }
}
