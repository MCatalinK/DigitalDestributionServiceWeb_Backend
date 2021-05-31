using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class ProductRepository : BaseRepository<ProductEntity>
    {
        public ProductRepository(DigitalDistributionDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<ProductEntity>> Search(string text)
        {
            return await Table
                .Where(p => p.Name.Contains(text))
                .ToListAsync();
        }
    }
}
