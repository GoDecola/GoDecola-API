using GoDecola.API.DTO.Request;
using GoDecola.API.DTO.Response;
using GoDecola.API.Mocks;
using GoDecola.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace GoDecola.API.Controller
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly List<MockUser> _mockUsers = new List<MockUser>
        {
            new MockUser { Email = "client@test.com", Password = "$2a$11$dZHH.pJw034edy91dsgxFOVFP.rytY1cC2UAGdhLspHLHDzgBNBx.", UserType = UserType.Cliente },
            new MockUser { Email = "admin@test.com", Password = "$2a$11$gWO97LXjLxufTjyywi2ckeUrG0rLLaKCeXgiEFOPx7lQxxSuquWLC", UserType = UserType.Admin },
            new MockUser { Email = "attendant@test.com", Password = "$2a$11$jK/7T7CXcGjEL.8nBoBzvuQfL021k/EIGFuff6D/EtmZcAxXrvlbW", UserType = UserType.Funcionario }
        };

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Task.Delay(100); // simula atraso 

            var userFound = _mockUsers.FirstOrDefault(u =>
                u.Email.Equals(loginRequestDTO.Email, StringComparison.OrdinalIgnoreCase)
            );

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
