using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class BaseRepository<T> where T : BaseEntity
    {
        protected readonly DigitalDistributionDbContext DbContext;
        protected readonly DbSet<T> Table;

        public BaseRepository(DigitalDistributionDbContext dbContext)
        {
            DbContext = dbContext;
            Table = DbContext.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return Table.Where(predicate);

            return Table;
        }

        public async Task<int> CountAll(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return await Table
                    .Where(predicate)
                    .CountAsync();

            return await Table.CountAsync();
        }

        public async Task Commit()
        {
            await DbContext.SaveChangesAsync();
        }

        public async Task<T> Create(T entity, bool commit = true)
        {
            await Table.AddAsync(entity);

            if (commit)
                await Commit();

            return entity;
        }

        public async Task<T> Update(T entity, bool commit = true)
        {
            Table.Update(entity);

            if (commit)
                await Commit();

            return entity;
        }

        public async Task<T> Delete(T entity, bool commit = true)
        {
            Table.Remove(entity);

            if (commit)
                await Commit();

            return entity;
        }
    }

}
