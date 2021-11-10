using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Providers.Abstract
{
    public abstract class EntityProvider<TContext, TEntity, TId>
        // Model of database table
        where TEntity : class

        // Database context
        where TContext : DbContext

        // TId - type if ID field 
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;

        protected EntityProvider(TContext context)
        {
            // database context
            this._context = context;

            // Now we get a DbSet from context by model 
            this._dbSet = context.Set<TEntity>();
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            // Gets all data from table
            // however point it as no tracking 
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<TEntity> GetById(TId id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> Get(Func<TEntity, bool> predicate)
        {
            // Gets data which are acceded by a query
            return (await GetAll()).Where(predicate).ToList();
        }

        public virtual async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task Add(TEntity added)
        {
            await _dbSet.AddAsync(added);
            await _context.SaveChangesAsync();
        }

        public virtual async Task AddRange(IEnumerable<TEntity> added)
        {
            await _dbSet.AddRangeAsync(added);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Edit(TEntity edited)
        {
            _context.Entry(edited).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public virtual async Task Remove(TEntity removed)
        {
            _dbSet.Remove(removed);

            await _context.SaveChangesAsync();
        }
    }

}
