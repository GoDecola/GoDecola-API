using GoDecola.API.Entities;
using System.Linq.Expressions;

namespace GoDecola.API.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<Review> GetByIdAsync(int id);
        Task<Review> UpdateAsync(Review review);
        Task<Review> AddAsync(Review review);
        Task DeleteAsync(int id);
        Task<IEnumerable<Review>> GetByPackageIdAsync(int travelPackageId);
        Task<IEnumerable<Review>> GetByUserIdAsync(string userId);
        Task<Review?> FindOneAsync(Expression<Func<Review, bool>> predicate);
    }
}
