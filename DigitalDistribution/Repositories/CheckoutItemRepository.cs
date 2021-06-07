using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
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
            var item = await _dbContext.InvoiceItems.Where(p => p.InvoiceId == invoice.Id && p.ProductId == product.Id).FirstOrDefaultAsync();

            if(item!=null)
            {
                _dbContext.InvoiceItems.Remove(item);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<CheckoutItemEntity> Create(InvoiceEntity invoice, ProductEntity product)
        {
            CheckoutItemEntity item = new()
            {
                InvoiceId = invoice.Id,
                ProductId = product.Id,
                Licence = HelperExtensionMethods.CreateLicence()
            };
            EntityEntry<CheckoutItemEntity> result = await _dbContext.InvoiceItems.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
    }
}
