using GoDecola.API.Data;
using GoDecola.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoDecola.API.Repositories
{
    public class TravelPackageRepository : IRepository<TravelPackage, int>
    {
        private readonly AppDbContext _context;

        public TravelPackageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TravelPackage>> GetAllAsync()
        {
            return await _context.TravelPackages
                .Include(tp => tp.Medias) // inclui as midias relacionadas
                .Include(tp => tp.AccommodationDetails) // inclui os detalhes de acomodação
                    .ThenInclude(ad => ad.Address) // inclui o endereço relacionado aos detalhes de acomodação
                .ToListAsync(); // lista todos os pacotes de viagem
        }

        public async Task<TravelPackage?> GetByIdAsync(int id)
        {
            return await _context.TravelPackages
                .Include(tp => tp.Medias) 
                .Include(tp => tp.AccommodationDetails) 
                    .ThenInclude(ad => ad.Address) 
                .FirstOrDefaultAsync(tp => tp.Id == id); // busca pelo Id
        }

        public async Task AddAsync(TravelPackage travelPackage)
        {
            await _context.TravelPackages.AddAsync(travelPackage);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TravelPackage travelPackage)
        {
            _context.TravelPackages.Update(travelPackage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var travelPackage = await GetByIdAsync(id);
            if (travelPackage != null)
            {
                _context.TravelPackages.Remove(travelPackage);
                await _context.SaveChangesAsync();
            }
        }

    }
}
