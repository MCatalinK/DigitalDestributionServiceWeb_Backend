using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using DigitalDistribution.Repositories;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Helpers;

namespace DigitalDistribution.Services
{
    public class BaseService<T> where T : BaseEntity
    {
        protected BaseRepository<T> BaseRepository;
        protected ClaimsPrincipal CurrentUser;

        public BaseService(BaseRepository<T> baseRepository,
            IHttpContextAccessor contextAccessor)
        {
            BaseRepository = baseRepository;
            CurrentUser = contextAccessor.HttpContext?.User;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            return BaseRepository.Get(predicate);
        }

        public async Task<int> CountAll(Expression<Func<T, bool>> predicate = null)
        {
            return await BaseRepository.CountAll(predicate);
        }

        public async Task Commit()
        {
            await BaseRepository.Commit();
        }

        public async Task<T> Create(T entity, bool commit = true)
        {
            return await BaseRepository.Create(entity, commit);
        }

        public async Task<T> Update(T entity, bool commit = true)
        {
            if (CurrentUser != null && CurrentUser.GetUserId() != 0)
            {
                entity.DateModified = DateTime.Now;
            }

            return await BaseRepository.Update(entity, commit);
        }

        public async Task<T> Delete(T entity, bool commit = true)
        {
            return await BaseRepository.Delete(entity, commit);
        }

    }
}
