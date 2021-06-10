using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class ReviewService
    {
        private readonly ReviewRepository _reviewRepository;
        protected ClaimsPrincipal CurrentUser;

        public ReviewService(ReviewRepository reviewRepository,
            IHttpContextAccessor contextAccessor)
        {
            _reviewRepository = reviewRepository;
            CurrentUser = contextAccessor.HttpContext?.User;
        }

        public IQueryable<ReviewEntity> Get(Expression<Func<ReviewEntity, bool>> predicate = null)
        {
            return _reviewRepository.Get(predicate);
        }

        public async Task Commit()
        {
            await _reviewRepository.Commit();
        }

        public async Task<ReviewEntity> Create(ReviewEntity entity, bool commit = true)
        {
            return await _reviewRepository.Create(entity, commit);
        }

        public async Task<ReviewEntity> Update(ReviewEntity entity, bool commit = true)
        {
            if (CurrentUser != null && CurrentUser.GetUserId() != 0)
            {
                entity.DateModified = DateTime.Now;
            }

            return await _reviewRepository.Update(entity, commit);
        }

        public async Task<ReviewEntity> Delete(ReviewEntity entity, bool commit = true)
        {
            return await _reviewRepository.Delete(entity, commit);
        }
    }
}
