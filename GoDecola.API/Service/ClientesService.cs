using GoDecola.API.Model;
using BCrypt.Net;

namespace GoDecola.API.Service
{
    public class ClientesService
    {
        public void CriarCliente(Clientes cliente)
        {

            cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);
        }
    }
}
