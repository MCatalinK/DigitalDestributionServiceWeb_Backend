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
        public UpdateService(UpdateRepository updateRepository,
           IHttpContextAccessor contextAccessor)
           : base(updateRepository, contextAccessor)
        {
            _updateRepository = updateRepository;
        }
    }
}
