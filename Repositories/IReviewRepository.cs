using System.Linq.Expressions;

namespace GoDecola.API.Repositories
{
    public interface IReviewRepository
    {
        Task<Review> AddAsync(Review review);
        Task<IEnumerable<Review>> GetByPackageIdAsync(int travelPackageId);
        Task<IEnumerable<Review>> GetByUserIdAsync(string userId);
        Task<Review?> FindOneAsync(Expression<Func<Review, bool>> predicate);
    }
}
