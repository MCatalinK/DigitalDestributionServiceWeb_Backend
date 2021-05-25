using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class ProfileService : BaseService<ProfileEntity>
    {
        public ProfileService(BaseRepository<ProfileEntity> profileEntityRepository,
            IHttpContextAccessor contextAccessor) 
            : base(profileEntityRepository, contextAccessor)
        {
        }
    }
}
