using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class CheckoutItemRepository
    {
        private readonly DigitalDistributionDbContext _dbContext;

        public CheckoutItemRepository(DigitalDistributionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CheckoutItemEntity>> GetAll()
        {
            return await _dbContext.InvoiceItems.ToListAsync();
        }
        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task<CheckoutItemEntity> Delete(CheckoutItemEntity item)
        {
            _dbContext.InvoiceItems.Remove(item);
            await Commit();

            return item;
        }
        public async Task<CheckoutItemEntity> Create(CheckoutItemEntity item)
        {
            EntityEntry<CheckoutItemEntity> result = await _dbContext.InvoiceItems.AddAsync(item);
            await Commit();
            return result.Entity;
        }
    }
}
