using AutoMapper;
using GoDecola.API.DTOs;
using GoDecola.API.DTOs.UserDTOs;
using GoDecola.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoDecola.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
         
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(CreateUserDTO registro)
        {
            var novoUsuario = new User
            {
                FirstName = registro.FirstName,
                LastName = registro.LastName,
                UserName = registro.Email,
                Email = registro.Email
            };

            var resultado = await _userManager.CreateAsync(novoUsuario, registro.Password!);

            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            return CreatedAtAction(nameof(GetById),
                new { id = novoUsuario.Id }, _mapper.Map<UserDTO>(novoUsuario));
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(usuarios));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN,User")]
        public async Task<ActionResult<UserDTO>> GetById(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            return Ok(_mapper.Map<UserDTO>(usuario));
        }

        // Atualiza os dados - exceto id e documento
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,User")]
        public async Task<IActionResult> Update(string id, UpdateUserDTO dados) 
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            if (usuario.Document != dados.Document)
                return BadRequest("Documento não pode ser alterado.");

            usuario.FirstName = dados.FirstName;
            usuario.LastName = dados.LastName;
            usuario.Email = dados.Email;
            usuario.UserName = dados.Email;

            var resultado = await _userManager.UpdateAsync(usuario);

            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteById(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            var resultado = await _userManager.DeleteAsync(usuario);

            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            return NoContent();
        }
    }
}
