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
            bool cpfValido = !string.IsNullOrWhiteSpace(register.CPF) && ValidationUtils.IsValidCPF(register.CPF);
            bool rneValido = !string.IsNullOrWhiteSpace(register.RNE) && ValidationUtils.IsValidRNE(register.RNE);
            bool passaportValido = !string.IsNullOrWhiteSpace(register.Passaport) && ValidationUtils.IsValidPassport(register.Passaport);

            if (!cpfValido && !rneValido && !passaportValido)
                return BadRequest("É obrigatório informar um CPF ou RNE válido.");
            if (!passaportValido)
                return BadRequest("Informe um passaporte válido.");

            // Verifica duplicidade de cpf
            if (cpfValido && await _userManager.Users.AnyAsync(u => u.CPF == register.CPF))
                return BadRequest("Já existe um usuário com este CPF.");

            // Verifica duplicidade de rne
            if (rneValido && await _userManager.Users.AnyAsync(u => u.RNE == register.RNE))
                return BadRequest("Já existe um usuário com este RNE.");

            // Verifica duplicidade de passaporteee
            if (passaportValido && await _userManager.Users.AnyAsync(u => u.Passaport == register.Passaport))
                return BadRequest("Já existe um usuário com este passaporte.");

            var user = new User
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.Email,
                Email = register.Email,
                CPF = register.CPF,
                RNE = register.RNE,
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
