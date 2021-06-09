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
    public class ReviewRepository:BaseRepository<ReviewEntity>
    {
        public ReviewRepository(DigitalDistributionDbContext dbContext):base(dbContext)
        {

        }


    }
}
