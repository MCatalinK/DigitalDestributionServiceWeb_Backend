using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{

    public class UpdateRepository : BaseRepository<UpdateEntity>
    {
        public UpdateRepository(DigitalDistributionDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<UpdateEntity> CreateUpdate(UpdateEntity entity, bool commit = true)
        {
            var products = DbContext.Set<ProductEntity>();
            var product = products.Where(p => p.Id == entity.ProductId).FirstOrDefault();
            product.Version = entity.Version;
            products.Update(product);

            await Table.AddAsync(entity);

            if (commit)
                await Commit();

            return entity;
        }
    }
}
