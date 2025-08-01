using GoDecola.API.Entities;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace GoDecola.API.Repositories
{
    public interface IReservationRepository : IRepository<Reservation, int>
    {
        Task<IEnumerable<Reservation>> GetByUserIdAsync(string userId);
        Task<Reservation?> FindOneWithDetailsAsync(Expression<Func<Reservation, bool>> predicate);
    }
}
