using GoDecola.API.DTO.Request;
using GoDecola.API.DTO.Response;
using GoDecola.API.Mocks;
using GoDecola.API.Model;
using GoDecola.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace GoDecola.API.Controller
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Task.Delay(100); // simula atraso 

            var userFound = _userService.GetUserByEmail(loginRequestDTO.Email);

            if (userFound == null)
            {
                return Unauthorized(new { message = "E-mail inválido" });
                
            }
            
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequestDTO.Password, userFound.Password);

            if (isPasswordValid)
            {
                string simulatedToken = $"mock_token_para_{userFound.UserType}_{DateTime.Now.Ticks}"; // simula token unico baseado no perfil e tempo

                var response = new LoginResponseDTO
                (
                    userFound.UserType,
                    simulatedToken
                );

                return Ok(response);
            }
            else
            {
                return Unauthorized(new { message = "Senha inválida" });
            }
        }
    }
}
