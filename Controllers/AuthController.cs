using AutoMapper;
using GoDecola.API.DTOs;
using GoDecola.API.DTOs.UserDTOs;
using GoDecola.API.Entities;
using GoDecola.API.Enums;
using GoDecola.API.Services;
using GoDecola.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GoDecola.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService; 
            _mapper = mapper;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(CreateUserDTO register)
        {
            // Documento: CPF e RNE 


            // vrifica se o campo do documento está preenchido
            if (string.IsNullOrWhiteSpace(register.Document))
                return BadRequest("Insira um CPF ou RNE válido.");

            string doc = register.Document.Trim();

            bool documentoValido = false;
            bool isCPF = false;
            string? CPF = null;
            string? RNE = null;

            // validação de cpf e rne com base no tamanho do documento
            if (doc.Length == 11)
            {
                documentoValido = ValidationUtils.IsValidCPF(doc);
                isCPF = true;
                CPF = doc;
            }
            else if (doc.Length == 9)
            {
                documentoValido = ValidationUtils.IsValidRNE(doc);
                isCPF = false;
                RNE = doc;
            }
            else
            {
                return BadRequest("É obrigatório informar um CPF ou RNE válido.");
            }

            // valdar se cpf/rne paassou na função de validação
            if (!documentoValido)
                return BadRequest("Documento inválido.");

            // Verifica duplicidade de cpf
            if (isCPF && await _userManager.Users.AnyAsync(u => u.CPF == CPF))
                return BadRequest("Já existe um usuário com este CPF.");

            // Verifica duplicidade de rne
            if (!isCPF && await _userManager.Users.AnyAsync(u => u.RNE == RNE))
                return BadRequest("Já existe um usuário com este RNE.");

            // Passaporte 
            bool passaportInformado = !string.IsNullOrWhiteSpace(register.Passaport);
            bool passaportValido = !passaportInformado || ValidationUtils.IsValidPassport(register.Passaport);

            // se passaporte for informado e não for válido, retorna erro
            if (passaportInformado && !passaportValido)
                return BadRequest("Informe um passaporte válido.");

            // Verifica duplicidade de passaporteee
            // se passaporte informado ele verifica se já existe um usuário com o mesmo passaporte, se nao for informado, não verifica
            if (passaportInformado && await _userManager.Users.AnyAsync(u => u.Passaport == register.Passaport))
                return BadRequest("Já existe um usuário com este passaporte.");

            var user = new User
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.Email,
                Email = register.Email,
                CPF = CPF, // se for rne, será null
                RNE = RNE, // se for cpf, será null
                Passaport = register.Passaport,
            };

            var result = await _userManager.CreateAsync(user, register.Password!);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "USER");
            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        [HttpPost("signin")] //login de usuário
        public async Task<IActionResult> SignIn(LoginRequestDTO login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email!);

            if (user == null)
            {
                return Unauthorized("Usuário inválido.");
            }

            var checagem = await _signInManager.CheckPasswordSignInAsync(user, login.Password!, false);

            if (!checagem.Succeeded)
            {
                return Unauthorized("Senha inválida");
            }

            var token = await _jwtService.GenerateToken(user);

            return Ok(
                new LoginResponseDTO
                {
                    Message = "Usuário autenticado com sucesso!",
                    Token = token,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                }
            );
        }

        [HttpPost("signout")] //logout do usuário
        [Authorize]
        public async Task<IActionResult> SignOut(LoginResponseDTO login)
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Usuário desconectado com sucesso!" });
        }
    }
}
