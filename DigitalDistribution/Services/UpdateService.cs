using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class UpdateService : BaseService<UpdateEntity>
    {
        private readonly UpdateRepository _updateRepository;
        public UpdateService(UpdateRepository updateRepository
            , IHttpContextAccessor contextAccessor)
            : base(updateRepository, contextAccessor)
        {
            _updateRepository = updateRepository;
        }

        public async Task<UpdateEntity> CreateUpdate(UpdateEntity entity, bool commit = true)
        {
            return await _updateRepository.CreateUpdate(entity,commit);
        }
    }
}
