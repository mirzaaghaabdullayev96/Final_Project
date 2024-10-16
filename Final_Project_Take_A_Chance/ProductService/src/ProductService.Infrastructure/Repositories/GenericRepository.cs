using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Repositories;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<T> Table => _dbContext.Set<T>();
        public virtual async Task<T?> GetAsync(int id)
        {
            var result = await Table.FindAsync(id);

            return result;
        }

        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> predicate,
                                                     Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)

        {
            IQueryable<T> query = Table;

            if (include != null) query = include(query);

            if (orderBy != null) query = orderBy(query);

            query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null,
                                                               Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                                               Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = Table;

            if (include != null) query = include(query);

            if (orderBy != null) query = orderBy(query);

            if (predicate != null) query = query.Where(predicate);

            return await query.ToListAsync();
        }

        public virtual async Task CreateAsync(T entity)
        {
            var entityEntry = await Table.AddAsync(entity);

            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task RemoveAsync(T entity)
        {
            var entityEntry = Table.Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var entityEntry = Table.Update(entity);

            await _dbContext.SaveChangesAsync();
        }
    }
}
