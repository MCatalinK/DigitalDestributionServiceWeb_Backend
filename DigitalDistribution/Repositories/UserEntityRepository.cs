using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class UserEntityRepository
    {
        private readonly DigitalDistributionDbContext _dbContext;

        public UserEntityRepository(DigitalDistributionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserEntity> Insert(UserEntity user)
        {
            var result = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<UserEntity> GetByUsername(string username)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(p => p.Username == username);
            return result;
        }
        public async Task<List<UserEntity>> GetAll()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<bool> Delete(string username)
        {
            var user = await GetByUsername(username);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<UserEntity> Update(UserEntity user)
        {
            var result = _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

    }
}
