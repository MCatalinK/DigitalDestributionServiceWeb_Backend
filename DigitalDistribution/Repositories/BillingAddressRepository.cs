using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class BillingAddressRepository : BaseRepository<BillingAddressEntity>
    {
        public BillingAddressRepository(DigitalDistributionDbContext dbContext) : base(dbContext)
        {

        }
    }
}
