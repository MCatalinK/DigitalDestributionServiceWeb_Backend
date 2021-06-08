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
        public async Task<List<InvoiceEntity>> GetBillsByPrice(UserEntity user,float lowerLimit, float upperLimit)
        {
            if (lowerLimit == 0)
                return await Table
                    .Where(p =>p.UserId==user.Id && p.Price <= upperLimit )
                    .ToListAsync();
            else if(upperLimit==0)
                return await Table
                    .Where(p =>p.UserId==user.Id && p.Price >=lowerLimit)
                    .ToListAsync();
            else
                return await Table
                    .Where(p =>p.UserId==user.Id && p.Price >= lowerLimit && p.Price<=upperLimit)
                    .ToListAsync();
        }
    }
}
