using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class BillingAddressRepository
    {
        private readonly DigitalDistributionDbContext _dbContext;
        private readonly DbSet<BillingAddressEntity> Table;
        public BillingAddressRepository(DigitalDistributionDbContext dbContext)
        {
            _dbContext = dbContext;
            Table = dbContext.Set<BillingAddressEntity>();
        }

        public IQueryable<BillingAddressEntity> Get(Expression<Func<BillingAddressEntity, bool>> predicate = null)
        {
            if (predicate != null)
                return Table.Where(predicate);

            return Table;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<BillingAddressEntity> Create(BillingAddressEntity entity, bool commit = true)
        {
            await Table.AddAsync(entity);

            if (commit)
                await Commit();

            return entity;
        }

        public async Task<BillingAddressEntity> Update(BillingAddressEntity entity, bool commit = true)
        {
            Table.Update(entity);

            if (commit)
                await Commit();

            return entity;
        }

        public async Task<BillingAddressEntity> Delete(BillingAddressEntity entity, bool commit = true)
        {
            Table.Remove(entity);

            if (commit)
                await Commit();

            return entity;
        }
    }
}
