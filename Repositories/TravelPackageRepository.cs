using GoDecola.API.Data;
using GoDecola.API.DTOs.TravelPackageDTOs;
using GoDecola.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        public async Task<IEnumerable<TravelPackage>> GetAllAsync(TravelPackageFilterDto? filter = null)
        {
            var query = _context.TravelPackages
                .Include(tp => tp.Medias)
                .Include(tp => tp.AccommodationDetails)
                    .ThenInclude(ad => ad.Address)
                .AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Destination))
                    query = query.Where(tp => tp.Destination!.Contains(filter.Destination));

                if (filter.StartDate.HasValue)
                    query = query.Where(tp => tp.StartDate >= filter.StartDate.Value);

                if (filter.EndDate.HasValue)
                    query = query.Where(tp => tp.EndDate <= filter.EndDate.Value);

                if (filter.NumberGuests.HasValue)
                    query = query.Where(tp => tp.NumberGuests >= filter.NumberGuests.Value);

                if (filter.PriceMin.HasValue)
                    query = query.Where(tp => tp.Price >= filter.PriceMin.Value);

                if (filter.PriceMax.HasValue)
                    query = query.Where(tp => tp.Price <= filter.PriceMax.Value);

                if (!string.IsNullOrEmpty(filter.Country))
                    query = query.Where(tp => tp.AccommodationDetails!.Address!.Country!.Contains(filter.Country));

                if (!string.IsNullOrEmpty(filter.State))
                    query = query.Where(tp => tp.AccommodationDetails!.Address!.State!.Contains(filter.State));

                if (!string.IsNullOrEmpty(filter.City))
                    query = query.Where(tp => tp.AccommodationDetails!.Address!.City!.Contains(filter.City));
            }

            return await query.ToListAsync();
        }


    }
}
