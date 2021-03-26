using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class UserEntityService
    {
        private readonly UserEntityRepository _userEntityReposityory;

        public UserEntityService(UserEntityRepository userEntityRepository)
        {
            _userEntityReposityory = userEntityRepository;
        }
        public async Task<UserEntity> CreateUser(UserEntity user)
        {
            UserEntity result = await _userEntityReposityory.Insert(user);
            return result;
        }

        public async Task<UserEntity> GetByUsername(string username)
        {
            return await _userEntityReposityory.GetByUsername(username);
        }

        public async Task<List<UserEntity>> GetAll()
        {
            return await _userEntityReposityory.GetAll();
        }

        public async Task<bool> Delete(string username)
        {
            return await _userEntityReposityory.Delete(username);
        }
        public async Task<UserEntity> Update(UserEntity user)
        {
            return await _userEntityReposityory.Update(user);
        }

    }
}
