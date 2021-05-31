using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class ProfileService : BaseService<ProfileEntity>
    {
        private readonly ProfileRepository _profileRepository;
        public ProfileService(ProfileRepository profileRepository,
            IHttpContextAccessor contextAccessor) 
            : base(profileRepository, contextAccessor)
        {
            _profileRepository = profileRepository;
        }
        public async Task<List<ProfileEntity>> Search(string text)
        {
            return await _profileRepository.Search(text);
        }
    }
}
