using GoDecola.API.Data;
using GoDecola.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoDecola.API.Repositories
{
    public class PaymentRepository : IRepository<Payment, int>
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .Include(p => p.Reservation) // inclui a reserva relacionada
                .ToListAsync(); // lista todos os pagamentos
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Reservation) // inclui a reserva relacionada
                .FirstOrDefaultAsync(p => p.Id == id); // busca pelo Id
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingPayment = await GetByIdAsync(id);
            if (existingPayment != null)
            {
                _context.Payments.Remove(existingPayment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
