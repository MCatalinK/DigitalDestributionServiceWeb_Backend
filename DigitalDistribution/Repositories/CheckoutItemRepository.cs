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
        public async Task<bool> Delete(InvoiceEntity invoice, ProductEntity product)
        {
            var item = await _dbContext.InvoiceItems
                .Where(p => p.InvoiceId == invoice.Id && p.ProductId == product.Id)
                .FirstOrDefaultAsync();

            if(item!=null)
            {
                invoice.Price -= product.Price;
                _dbContext.InvoiceItems.Remove(item);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<CheckoutItemEntity> Create(CheckoutItemEntity item)
        {
            var user = await _dbContext.Users
                .Include(p=>p.Bills.Where(p => p.Id == item.InvoiceId))
                .ThenInclude(p => p.CheckoutItems)
                .FirstOrDefaultAsync();

                user.Bills.First().CheckoutItems.Add(item);

            var product = await _dbContext.Products
                .Where(p => p.Id == item.ProductId)
                .Include(p=>p.InvoiceItems)
                .FirstOrDefaultAsync();


                user.Bills.First().Price += product.Price;

            product.InvoiceItems.Add(item);

            EntityEntry<CheckoutItemEntity> result = await _dbContext.InvoiceItems.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
    }
}
