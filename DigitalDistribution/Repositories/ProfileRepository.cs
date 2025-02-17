﻿using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class ProfileRepository : BaseRepository<ProfileEntity>
    {
        public ProfileRepository(DigitalDistributionDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<ProfileEntity>>Search(string text)
        {
            return await Table
                .Where(p => p.DisplayName.Contains(text))
                .ToListAsync();
        }
    }
}
