using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
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
            await Table.AddAsync(entity);

            if (commit)
                await Commit();

            return entity;
        }
    }
}
