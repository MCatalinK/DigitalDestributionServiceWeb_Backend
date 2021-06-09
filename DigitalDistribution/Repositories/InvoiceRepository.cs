using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class InvoiceRepository
    {
        private readonly DigitalDistributionDbContext _dbContext;
        private readonly DbSet<InvoiceEntity> Table;

        public InvoiceRepository(DigitalDistributionDbContext dbContext)
        {
            _dbContext = dbContext;
            Table = dbContext.Set<InvoiceEntity>();
        }
     
        public IQueryable<InvoiceEntity> Get(Expression<Func<InvoiceEntity, bool>> predicate = null)
        {
            if (predicate != null)
                return Table.Where(predicate);

            return Table;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<InvoiceEntity> Create(InvoiceEntity entity, bool commit = true)
        {
            await Table.AddAsync(entity);

            if (commit)
                await Commit();

            return entity;
        }

        public async Task<InvoiceEntity> Update(InvoiceEntity entity, bool commit = true)
        {
            Table.Update(entity);

            if (commit)
                await Commit();

            return entity;
        }

        public async Task<InvoiceEntity> Delete(InvoiceEntity entity, bool commit = true)
        {
            Table.Remove(entity);

            if (commit)
                await Commit();

            return entity;
        }

    }
}
