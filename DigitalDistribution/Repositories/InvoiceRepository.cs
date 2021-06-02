using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;

namespace DigitalDistribution.Repositories
{
    public class InvoiceRepository : BaseRepository<InvoiceEntity>
    {
        public InvoiceRepository(DigitalDistributionDbContext dbContext) : base(dbContext)
        {
        }
        
    }
}
