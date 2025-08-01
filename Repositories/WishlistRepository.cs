using GoDecola.API.Data;
using GoDecola.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoDecola.API.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Wishlist>> GetAllAsync()
        {
            return await _context.Wishlists
                .Include(w => w.TravelPackage)
                .Include(w => w.User)
                .ToListAsync();
        }

        public async Task<Wishlist?> GetByIdAsync(int id)
        {
            return await _context.Wishlists
                .Include(w => w.TravelPackage)
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddAsync(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Wishlist wishlist)
        {
            _context.Wishlists.Update(wishlist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var wishlist = await GetByIdAsync(id);
            if (wishlist != null)
            {
                _context.Wishlists.Remove(wishlist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Wishlist?> GetByUserAndPackageAsync(string userId, int travelPackageId)
        {
            return await _context.Wishlists
                .Include(w => w.TravelPackage)
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.UserId == userId && w.TravelPackageId == travelPackageId);
        }

        public async Task<IEnumerable<Wishlist>> GetAllByUserAsync(string userId)
        {
            return await _context.Wishlists
                .Include(w => w.TravelPackage)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }
    }
}
