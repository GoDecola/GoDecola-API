using GoDecola.API.Data;
using GoDecola.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GoDecola.API.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.User) // inclui o usuario que fez a avaliacao
                .Include(r => r.TravelPackage) // inclui dados do pacote
                .OrderByDescending(r => r.ReviewDate) // ordena as reviews pela data, da mais recente para a mais antiga
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.User) // inclui o usuario que fez a avaliacao
                .Include(r => r.TravelPackage) // inclui dados do pacote
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<IEnumerable<Review>> GetByPackageIdAsync(int travelPackageId)
        {
            return await _context.Reviews
                .Include(r => r.User) // inclui o usuario que fez a avaliacao
                .Where(r => r.TravelPackageId == travelPackageId)
                .OrderByDescending(r => r.ReviewDate) // ordena as reviews pela data, da mais recente para a mais antiga
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByUserIdAsync(string userId)
        {
            return await _context.Reviews
                .Include(r => r.TravelPackage) // inclui dados do pacote
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<Review?> FindOneAsync(Expression<Func<Review, bool>> predicate)
        {
            return await _context.Reviews.FirstOrDefaultAsync(predicate);
        }
    }
}
