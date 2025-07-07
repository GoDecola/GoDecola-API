using GoDecola.API.Model;
using BCrypt.Net;

namespace GoDecola.API.Service
{
    public class UserService
    {
        public void CriarCliente(User user)
        {

            user.Senha = BCrypt.Net.BCrypt.HashPassword(user.Senha);
        }
    }
}
