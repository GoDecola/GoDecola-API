using GoDecola.API.Entities;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Linq.Expressions;

namespace GoDecola.API.Repositories
{
    public interface IRepository<T, TId> where T : class
    {
        Task<T?> GetByIdAsync(TId id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TId id);
        
    }
}
