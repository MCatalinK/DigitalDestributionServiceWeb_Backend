using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;

namespace DigitalDistribution.Services
{
    public class DevelopmentTeamService : BaseService<DevelopmentTeamEntity>
    {
        public DevelopmentTeamService(DevelopmentTeamRepository developerTeamRepository,
            IHttpContextAccessor contextAccessor)
            : base(developerTeamRepository, contextAccessor)
        {
        }
    }
}
