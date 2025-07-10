using GoDecola.API.Mocks;
using GoDecola.API.Model;

namespace GoDecola.API.Service
{
    public class MockUserService : IUserService
    {
        private readonly List<MockUser> _mockUsers = new List<MockUser>
        {
            new MockUser { Email = "client@test.com", Password = "$2a$11$dZHH.pJw034edy91dsgxFOVFP.rytY1cC2UAGdhLspHLHDzgBNBx.", UserType = UserType.Cliente },
            new MockUser { Email = "admin@test.com", Password = "$2a$11$gWO97LXjLxufTjyywi2ckeUrG0rLLaKCeXgiEFOPx7lQxxSuquWLC", UserType = UserType.Admin },
            new MockUser { Email = "attendant@test.com", Password = "$2a$11$jK/7T7CXcGjEL.8nBoBzvuQfL021k/EIGFuff6D/EtmZcAxXrvlbW", UserType = UserType.Funcionario }
        };

        public MockUser? GetUserByEmail(string email)
        {
            return _mockUsers.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
