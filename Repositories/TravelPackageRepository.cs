using GoDecola.API.Data;
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
                .Include(tp => tp.Reviews)
                .ToListAsync(); // lista todos os pacotes de viagem
        }

        public async Task<TravelPackage?> GetByIdAsync(int id)
        {
            return await _context.TravelPackages
                .Include(tp => tp.Medias) 
                .Include(tp => tp.AccommodationDetails) 
                    .ThenInclude(ad => ad.Address)
                .Include(tp => tp.Reviews)
                .FirstOrDefaultAsync(tp => tp.Id == id); // busca pelo Id
        }

        public async Task AddAsync(TravelPackage travelPackage)
        {
            await _context.TravelPackages.AddAsync(travelPackage);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TravelPackage travelPackage)
        {
            // Fetch the existing package with related data
            var existingPackage = await _context.TravelPackages
                .Include(tp => tp.AccommodationDetails)
                .ThenInclude(ad => ad.Address)
                .FirstOrDefaultAsync(tp => tp.Id == travelPackage.Id);

            if (existingPackage == null)
            {
                throw new KeyNotFoundException("Travel package not found.");
            }

            // Update scalar properties
            existingPackage.Title = travelPackage.Title;
            existingPackage.Description = travelPackage.Description;
            existingPackage.Price = travelPackage.Price;
            existingPackage.Destination = travelPackage.Destination;
            existingPackage.StartDate = travelPackage.StartDate;
            existingPackage.EndDate = travelPackage.EndDate;
            existingPackage.NumberGuests = travelPackage.NumberGuests;
            existingPackage.IsActive = travelPackage.IsActive;            
            existingPackage.DiscountPercentage = travelPackage.DiscountPercentage;
            existingPackage.PromotionStartDate = travelPackage.PromotionStartDate;
            existingPackage.PromotionEndDate = travelPackage.PromotionEndDate;
            existingPackage.PackageType = travelPackage.PackageType;

            // Handle AccommodationDetails
            if (travelPackage.AccommodationDetails == null)
            {
                throw new InvalidOperationException("AccommodationDetails is required.");
            }

            if (existingPackage.AccommodationDetails == null)
            {
                existingPackage.AccommodationDetails = new AccommodationDetails();
            }

            var accommodationDetails = existingPackage.AccommodationDetails;
            accommodationDetails.NumberBaths = travelPackage.AccommodationDetails.NumberBaths;
            accommodationDetails.NumberBeds = travelPackage.AccommodationDetails.NumberBeds;
            accommodationDetails.HasWifi = travelPackage.AccommodationDetails.HasWifi;
            accommodationDetails.HasParking = travelPackage.AccommodationDetails.HasParking;
            accommodationDetails.HasPool = travelPackage.AccommodationDetails.HasPool;
            accommodationDetails.HasGym = travelPackage.AccommodationDetails.HasGym;
            accommodationDetails.HasRestaurant = travelPackage.AccommodationDetails.HasRestaurant;
            accommodationDetails.HasPetFriendly = travelPackage.AccommodationDetails.HasPetFriendly;
            accommodationDetails.HasAirConditioning = travelPackage.AccommodationDetails.HasAirConditioning;
            accommodationDetails.HasBreakfastIncluded = travelPackage.AccommodationDetails.HasBreakfastIncluded;

            // Handle Address
            if (travelPackage.AccommodationDetails.Address == null)
            {
                throw new InvalidOperationException("Address is required.");
            }

            Address existingAddress = null;
            var incomingAddress = travelPackage.AccommodationDetails.Address;

            // Check if the provided Address.Id is valid
            if (incomingAddress.Id > 0)
            {
                existingAddress = await _context.Addresses.FindAsync(incomingAddress.Id);
                if (existingAddress == null)
                {
                    throw new InvalidOperationException($"Address with Id {incomingAddress.Id} not found.");
                }
                // Update existing address fields
                existingAddress.AddressLine1 = incomingAddress.AddressLine1;
                existingAddress.AddressLine2 = incomingAddress.AddressLine2;
                existingAddress.ZipCode = incomingAddress.ZipCode;
                existingAddress.Country = incomingAddress.Country;
                existingAddress.State = incomingAddress.State;
                existingAddress.City = incomingAddress.City;
                existingAddress.Neighborhood = incomingAddress.Neighborhood;
                existingAddress.Latitude = incomingAddress.Latitude;
                existingAddress.Longitude = incomingAddress.Longitude;
            }
            else
            {
                // Check if an address with the same details exists
                existingAddress = await _context.Addresses
                    .FirstOrDefaultAsync(a =>
                        a.ZipCode == incomingAddress.ZipCode &&
                        a.AddressLine1 == incomingAddress.AddressLine1 &&
                        a.AddressLine2 == incomingAddress.AddressLine2 &&
                        a.City == incomingAddress.City &&
                        a.State == incomingAddress.State &&
                        a.Country == incomingAddress.Country);

                if (existingAddress == null)
                {
                    // Create new Address
                    existingAddress = new Address
                    {
                        AddressLine1 = incomingAddress.AddressLine1,
                        AddressLine2 = incomingAddress.AddressLine2,
                        ZipCode = incomingAddress.ZipCode,
                        Country = incomingAddress.Country,
                        State = incomingAddress.State,
                        City = incomingAddress.City,
                        Neighborhood = incomingAddress.Neighborhood,
                        Latitude = incomingAddress.Latitude,
                        Longitude = incomingAddress.Longitude
                    };
                    _context.Addresses.Add(existingAddress);
                    await _context.SaveChangesAsync(); // Save to generate AddressId
                }
            }

            // Link Address to AccommodationDetails
            accommodationDetails.AddressId = existingAddress.Id;
            accommodationDetails.Address = existingAddress;

            // Update the context
            _context.TravelPackages.Update(existingPackage);
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
