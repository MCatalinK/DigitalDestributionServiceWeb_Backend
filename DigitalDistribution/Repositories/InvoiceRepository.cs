using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
