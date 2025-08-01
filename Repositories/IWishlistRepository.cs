using GoDecola.API.Entities;

namespace GoDecola.API.Repositories
{
    public interface IWishlistRepository : IRepository<Wishlist, int>
    {
        Task<Wishlist?> GetByUserAndPackageAsync(string userId, int travelPackageId);
        Task<IEnumerable<Wishlist>> GetAllByUserAsync(string userId);
    }

}