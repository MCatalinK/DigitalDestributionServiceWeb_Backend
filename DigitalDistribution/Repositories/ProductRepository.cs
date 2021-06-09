using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class ProductRepository
    {
        private readonly DigitalDistributionDbContext _dbContext;
        private readonly DbSet<ProductEntity> Table;

        public ProductRepository(DigitalDistributionDbContext dbContext)
        {
            _dbContext = dbContext;
            Table = dbContext.Set<ProductEntity>();
        }

        public IQueryable<ProductEntity> Get(Expression<Func<ProductEntity, bool>> predicate = null)
        {
            if (predicate != null)
                return Table.Where(predicate);

            return Table;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProductEntity> Create(ProductEntity entity, bool commit = true)
        {
            await Table.AddAsync(entity);

            if (commit)
                await Commit();

            return entity;
        }

        public async Task<ProductEntity> Update(ProductEntity entity, bool commit = true)
        {
            Table.Update(entity);

            if (commit)
                await Commit();

            return entity;
        }

        public async Task<ProductEntity> Delete(ProductEntity entity, bool commit = true)
        {
            entity.IsDeleted = true;

            var reviews = _dbContext.Set<ReviewEntity>();
            reviews.Where(p => p.ProductId == entity.Id).ToList();
            foreach (var review in reviews)
                reviews.Remove(review);

            var updates = _dbContext.Set<UpdateEntity>();
            updates.Where(p => p.ProductId == entity.Id).ToList();

            foreach (var update in updates)
            {
                update.IsDeleted = true;
                updates.Update(update);
            }

            if (commit)
                await Commit();

            Table.Update(entity);

            return entity;
        }

        public async Task<ProductEntity> Search(int id)
        {
            return await Table
                .Where(p => p.Id==id)
                .FirstOrDefaultAsync();
        }
        public async Task<List<ProductEntity>> Search(string text)
        {
            return await Table
                .Where(p => p.Name.Contains(text))
                .ToListAsync();
        }
        public async Task<List<ProductEntity>> GetProductByPrice(float upperLimit,float lowerLimit)
        {
            return await Table
                .Where(p => p.Price >= lowerLimit && p.Price <= upperLimit)
                .ToListAsync();
        }
        public async Task<List<ProductEntity>> GetProductByRating(int minRating)
        {
            return await Table
                .Where(p => p.Rating >= minRating)
                .ToListAsync();
        }
    }
}
