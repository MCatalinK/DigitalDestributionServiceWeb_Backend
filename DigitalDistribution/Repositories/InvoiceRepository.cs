using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using System;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class InvoiceRepository : BaseRepository<InvoiceEntity>
    {

        public InvoiceRepository(DigitalDistributionDbContext dbContext) : base(dbContext)
        {
        }
       
    }
}
