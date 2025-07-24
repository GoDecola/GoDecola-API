using AutoMapper;
using GoDecola.API.DTOs;
using GoDecola.API.DTOs.UserDTOs;
using GoDecola.API.Entities;
using GoDecola.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoDecola.API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost("signup")] //register de usuário
        public async Task<IActionResult> SignUp(CreateUserDTO register)
        {
            var user = new User
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.Email,
                Email = register.Email,
            };

            var result = await _userManager.CreateAsync(user, register.Password!);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

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
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Usuário desconectado com sucesso!" });
        }
    }
}
