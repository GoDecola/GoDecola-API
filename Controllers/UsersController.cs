using AutoMapper;
using GoDecola.API.Data;
using GoDecola.API.DTOs;
using GoDecola.API.DTOs.ReservationDTOs;
using GoDecola.API.DTOs.UserDTOs;
using GoDecola.API.Entities;
using GoDecola.API.Enums;
using GoDecola.API.Repositories;
using GoDecola.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(AdminCreateUserDTO create)
        {
            // validar cpf/rne do administrador
            if (string.IsNullOrWhiteSpace(create.Document))
                return BadRequest("Insira um CPF ou RNE válido.");

            string doc = create.Document.Trim();

            bool documentoValido = false;
            bool isCPF = false;
            string? CPF = null;
            string? RNE = null;

            if (doc.Length == 11)
            {
                documentoValido = ValidationUtils.IsValidCPF(doc);
                isCPF = true;
                CPF = doc;
            }
            else if (doc.Length == 8)
            {
                documentoValido = ValidationUtils.IsValidRNE(doc);
                RNE = doc;
            }
            else
            {
                return BadRequest("É obrigatório informar um CPF ou RNE válido.");
            }

            if (!documentoValido)
                return BadRequest("Documento inválido.");

            if (isCPF && await _userManager.Users.AnyAsync(u => u.CPF == CPF))
                return BadRequest("Já existe um usuário com este CPF.");

            if (!isCPF && await _userManager.Users.AnyAsync(u => u.RNE == RNE))
                return BadRequest("Já existe um usuário com este RNE.");


            var newUser = new User
            {
                FirstName = create.FirstName,
                LastName = create.LastName,
                UserName = create.Email,
                Email = create.Email,
                CPF = create.CPF,
                RNE = create.RNE,
                Passaport = create.Passaport,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(newUser, create.Password!);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(newUser, create.Role.ToUpper());

            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return CreatedAtAction(nameof(GetByIdOrDocument),
                new { idOrDocument = newUser.Id },
                new
                {
                    message = "Usuário cadastrado com sucesso!",
                    user = _mapper.Map<UserDTO>(newUser)
                });
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
        }

        [HttpGet("{idOrDocument}")]
        [Authorize(Roles = "ADMIN, SUPPORT, USER")]
        public async Task<ActionResult<UserDTO>> GetByIdOrDocument(string idOrDocument)
        {
            User? users;

            // tenta buscar por id ou cpf/rnee

            if (Guid.TryParse(idOrDocument, out _))
            {
                users = await _userManager.FindByIdAsync(idOrDocument);
            }
            else
            {
                string doc = idOrDocument.Trim();

                if (doc.Length == 11)
                {
                    users = await _context.Users.FirstOrDefaultAsync(u => u.CPF == doc);
                }
                else if (doc.Length == 8)
                {
                    users = await _context.Users.FirstOrDefaultAsync(u => u.RNE == doc);
                }
                else
                {
                    return BadRequest("Informe um ID, CPF ou RNE válido.");
                }
            }

            if (users == null)
                return NotFound("Usuário não encontrado.");

            return Ok(_mapper.Map<UserDTO>(users));
        }

        // Atualiza os dados - exceto documento
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN, SUPPORT, USER")]
        public async Task<IActionResult> Update(string id, UpdateUserDTO dados)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            // verifica tentativa de alterar cpf ou rne
            if (!string.IsNullOrWhiteSpace(dados.Document))
            {
                string doc = dados.Document.Trim();

                if (doc.Length == 11 && usuario.CPF != doc)
                    return BadRequest("CPF não pode ser alterado.");

                if (doc.Length == 9 && usuario.RNE != doc)
                    return BadRequest("RNE não pode ser alterado.");
            }

            // atualiza dados permitidos 
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
        [Authorize(Roles = "ADMIN, SUPPORT, USER")]
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
        [Authorize(Roles = "ADMIN, SUPPORT, USER")]
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
