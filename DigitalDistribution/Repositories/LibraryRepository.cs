using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class LibraryRepository
    {
        private readonly DigitalDistributionDbContext _dbContext;

        public LibraryRepository(DigitalDistributionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LibraryProductEntity>> GetAll()
        {
            return await _dbContext.LibraryItems.ToListAsync();
        }

        public async Task<bool> Create(UserEntity user ,InvoiceEntity invoice)
        {
            foreach(var item in invoice.CheckoutItems)
            {
                await _dbContext.LibraryItems.AddAsync(new LibraryProductEntity
                {
                    DateAdded = DateTime.Now,
                    UserId = user.Id,
                    ProductId = item.ProductId
                });
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
