using GoDecola.API.Mocks;

namespace GoDecola.API.Service
{
    public interface IUserService
    {
        MockUser GetUserByEmail(string email);
    }
}
