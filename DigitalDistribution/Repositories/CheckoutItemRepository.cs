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
            var invoices = _dbContext.Set<InvoiceEntity>();
            var invoice = invoices.Where(p => p.Id == item.InvoiceId).FirstOrDefault();
            var products = _dbContext.Set<ProductEntity>();
            var product = products.Where(p => p.Id == item.ProductId).FirstOrDefault();
            invoice.Price -= product.Price;

            invoices.Update(invoice);
            _dbContext.InvoiceItems.Remove(item);
            await Commit();

            return item;
        }
        public async Task<CheckoutItemEntity> Create(int userId,CheckoutItemEntity item)
        {
            var users = _dbContext.Set<UserEntity>();

            var user = await users.Where(p=>p.Id==userId)
                .Include(p=>p.Bills.Where(p => p.Id == item.InvoiceId))
                .ThenInclude(p => p.CheckoutItems)
                .FirstOrDefaultAsync();

            var products= _dbContext.Set<ProductEntity>();

            var product = await products
                .Where(p => p.Id == item.ProductId)
                .Include(p=>p.InvoiceItems)
                .FirstOrDefaultAsync();

                user.Bills.First().Price += product.Price;

            var invoices = _dbContext.Set<InvoiceEntity>();
            invoices.Update(user.Bills.First());

            product.InvoiceItems.Add(item);

            EntityEntry<CheckoutItemEntity> result = await _dbContext.InvoiceItems.AddAsync(item);
            await Commit();
            return result.Entity;
        }
    }
}
