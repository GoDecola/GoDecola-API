using GoDecola.API.Data;
using GoDecola.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoDecola.API.Repositories
{
    public class ReservationRepository : IRepository<Reservation, int>
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.User) // inclui o usuário relacionado
                .Include(r => r.TravelPackage) // inclui o pacote de viagem relacionado
                .Include(r => r.Guests) // inclui os hóspedes relacionados
                .ToListAsync(); // lista todas as reservas
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.User) // inclui o usuário relacionado
                .Include(r => r.TravelPackage) // inclui o pacote de viagem relacionado
                .Include(r => r.Guests) // inclui os hóspedes relacionados
                .FirstOrDefaultAsync(r => r.Id == id); // busca pela Id
        }

        public async Task AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }   

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            var existingReservation = await GetByIdAsync(reservation.Id);
            if (existingReservation != null)
            {
                _context.Reservations.Remove(existingReservation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
