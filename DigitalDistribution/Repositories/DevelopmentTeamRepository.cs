using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;

namespace DigitalDistribution.Repositories
{
    public class DevelopmentTeamRepository : BaseRepository<DevelopmentTeamEntity>
    {
        public DevelopmentTeamRepository(DigitalDistributionDbContext dbContext) : base(dbContext)
        {
        }
    }
}
