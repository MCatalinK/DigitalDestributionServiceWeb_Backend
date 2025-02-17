﻿using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class ReviewRepository
    {
        private readonly DigitalDistributionDbContext _dbContext;
        private readonly DbSet<ReviewEntity> Table;
        public ReviewRepository(DigitalDistributionDbContext dbContext)
        {
            _dbContext = dbContext;
            Table = dbContext.Set<ReviewEntity>();
        }
        
        public IQueryable<ReviewEntity> Get(Expression<Func<ReviewEntity, bool>> predicate = null)
        {
            if (predicate != null)
                return Table.Where(predicate);

            return Table;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ReviewEntity> Create(ReviewEntity entity, bool commit = true)
        {
            await Table.AddAsync(entity);

            if (commit)
                await Commit();

            return entity;
        }

        public async Task<ReviewEntity> Update(ReviewEntity entity, bool commit = true)
        {
            Table.Update(entity);

            if (commit)
                await Commit();

            return entity;
        }

        public async Task<ReviewEntity> Delete(ReviewEntity entity, bool commit = true)
        {
            Table.Remove(entity);

            if (commit)
                await Commit();

            return entity;
        }
    }
}
