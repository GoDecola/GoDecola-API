using GoDecola.API.DTO;
using GoDecola.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace GoDecola.API.Controller
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly List<dynamic> _mockUsers = new List<dynamic>
        {
            new { Email = "client@test.com", Password = "$2a$11$dZHH.pJw034edy91dsgxFOVFP.rytY1cC2UAGdhLspHLHDzgBNBx.", UserType = "Client" },
            new { Email = "admin@test.com", Password = "$2a$11$gWO97LXjLxufTjyywi2ckeUrG0rLLaKCeXgiEFOPx7lQxxSuquWLC", UserType = "Administrator" },
            new { Email = "attendant@test.com", Password = "$2a$11$jK/7T7CXcGjEL.8nBoBzvuQfL021k/EIGFuff6D/EtmZcAxXrvlbW", UserType = "Attendant" }
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

                return Ok(new
                {
                    userType = userFound.UserType,
                    token = simulatedToken
                });
            }
            else
            {
                return Unauthorized(new { message = "Senha inválida" });
            }
        }
    }
}
