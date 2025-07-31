using AutoMapper;
using GoDecola.API.Data;
using GoDecola.API.DTOs;
using GoDecola.API.DTOs.ReservationDTOs;
using GoDecola.API.DTOs.UserDTOs;
using GoDecola.API.Entities;
using GoDecola.API.Enums;
using GoDecola.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoDecola.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly ReservationRepository _reservationRepository;

        public UsersController(UserManager<User> userManager, IMapper mapper, AppDbContext context, ReservationRepository reservationRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _reservationRepository = reservationRepository;
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserType.ADMIN))]
        public async Task<IActionResult> Create(AdminCreateUserDTO create)
        {
            if (string.IsNullOrWhiteSpace(create.Role))
                return BadRequest("A role é obrigatória (ADMIN, SUPPORT ou USER).");

            var novoUsuario = new User
            {
                FirstName = create.FirstName,
                LastName = create.LastName,
                UserName = create.Email,
                Email = create.Email,
                CPF = create.CPF,
                RNE = create.RNE,
                Passaport = create.Passaport
            };

            var resultado = await _userManager.CreateAsync(novoUsuario, create.Password!);

            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            var roleResult = await _userManager.AddToRoleAsync(novoUsuario, create.Role.ToUpper());

            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return CreatedAtAction(nameof(GetByIdOrDocument),
                new { idOrDocument = novoUsuario.Id },
                new
                {
                    message = "Usuário cadastrado com sucesso!",
                    user = _mapper.Map<UserDTO>(novoUsuario)
                });
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserType.ADMIN))]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(usuarios));
        }
        // passar as roles de string para enum
        [HttpGet("{idOrDocument}")]
        [Authorize(Roles = $"{nameof(UserType.ADMIN)}, {nameof(UserType.SUPPORT)}, {nameof(UserType.USER)}")]
        public async Task<ActionResult<UserDTO>> GetByIdOrDocument(string idOrDocument)
        {
            User? usuario;

            // Tenta buscar por ID
            if (Guid.TryParse(idOrDocument, out var guid))
            {
                usuario = await _userManager.FindByIdAsync(idOrDocument);
            }
            else
            {
                // Se não for, tenta por CPF ou RNE
                usuario = await _context.Users
                    .FirstOrDefaultAsync(u => u.CPF == idOrDocument || u.RNE == idOrDocument);
            }

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            return Ok(_mapper.Map<UserDTO>(usuario));
        }

        // Atualiza os dados - exceto documento
        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(UserType.ADMIN)}, {nameof(UserType.SUPPORT)}, {nameof(UserType.USER)}")]
        public async Task<IActionResult> Update(string id, UpdateUserDTO dados)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            if (usuario.CPF != dados.CPF || usuario.RNE != dados.RNE)
                return BadRequest("CPF ou RNE não podem ser alterados.");

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
        [Authorize(Roles = $"{nameof(UserType.ADMIN)}, {nameof(UserType.SUPPORT)}, {nameof(UserType.USER)}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Usuário não encontrado.");

            var reservations = await _context.Reservations
                .Where(r => r.UserId == id)
                .ToListAsync();

            if (reservations.Count == 0)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                return Ok("Usuário excluído com sucesso.");
            }

            bool allUsed = reservations.All(r => r.Status == ReservationStatus.USED);

            if (!allUsed)
                return BadRequest("Usuário não pode ser excluído: Há reservas ativas ainda não utilizadas.");

            var deleteResult = await _userManager.DeleteAsync(user);

            if (!deleteResult.Succeeded)
                return BadRequest(deleteResult.Errors);

            return Ok("Usuário excluído com sucesso.");
        }
        [HttpGet("{userId}/reservations")]
        [Authorize(Roles = $"{nameof(UserType.ADMIN)}, {nameof(UserType.SUPPORT)}, {nameof(UserType.USER)}")]
        public async Task<IActionResult> GetUserReservations(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Usuário não encontrado.");

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(nameof(UserType.ADMIN));
            var isSupport = User.IsInRole(nameof(UserType.SUPPORT));

            // Permite acesso se for Admin, Support ou o próprio usuário
            if (!isAdmin && !isSupport && currentUserId != userId)
                return Forbid("Você não tem permissão para acessar essas reservas.");

            var reservations = await _reservationRepository.GetByUserIdAsync(userId);
            var reservationDTOs = _mapper.Map<IEnumerable<ReservationDTO>>(reservations);

            return Ok(reservationDTOs);
        }
    }
}
