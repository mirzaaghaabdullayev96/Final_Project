using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public DbSet<T> Table { get; }
        Task<T?> GetAsync(int id);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate,
                          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null,
                                          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task CreateManyAsync(ICollection<T> entities);
    }
}
