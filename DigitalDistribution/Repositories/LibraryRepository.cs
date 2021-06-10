using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DigitalDistribution.Repositories
{
    public class LibraryRepository
    {
        private readonly DigitalDistributionDbContext _dbContext;
        private readonly DbSet<LibraryProductEntity> Table;

        public LibraryRepository(DigitalDistributionDbContext dbContext)
        {
            _dbContext = dbContext;
            Table = _dbContext.Set<LibraryProductEntity>();
        }
        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<LibraryProductEntity> Get(Expression<Func<LibraryProductEntity, bool>> predicate = null)
        {
            if (predicate != null)
                return Table.Where(predicate);

            return Table;
        }
        public async Task<bool> Create(List<LibraryProductEntity> library)
        {

            foreach (var item in library)
                Table.Add(item);
            await Commit();
            return true;
        }
    }
}
