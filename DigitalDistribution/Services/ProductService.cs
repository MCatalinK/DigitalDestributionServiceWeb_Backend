using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        protected ClaimsPrincipal CurrentUser;
        public ProductService(ProductRepository productRepository,
            IHttpContextAccessor contextAccessor)
        {
            _productRepository = productRepository;
            CurrentUser = contextAccessor.HttpContext?.User;
        }

        public IQueryable<ProductEntity> Get(Expression<Func<ProductEntity, bool>> predicate = null)
        {
            return _productRepository.Get(predicate);
        }

        public async Task Commit()
        {
            await _productRepository.Commit();
        }

        public async Task<ProductEntity> Create(ProductEntity entity, bool commit = true)
        {
            if (CurrentUser != null && CurrentUser.GetUserId() != 0)
                entity.CreatedBy = CurrentUser.GetUserId();

            return await _productRepository.Create(entity, commit);
        }

        public async Task<ProductEntity> Update(ProductEntity entity, bool commit = true)
        {
            if (CurrentUser != null && CurrentUser.GetUserId() != 0)
            {
                entity.DateModified = DateTime.Now;
            }

            return await _productRepository.Update(entity, commit);
        }

        public async Task<ProductEntity> Delete(ProductEntity entity, bool commit = true)
        {
            return await _productRepository.Delete(entity, commit);
        }

        public async Task<ProductEntity> Search(int id)
        {
            return await _productRepository.Search(id);
        }

        public async Task<List<ProductEntity>> Search(string text)
        {
            return await _productRepository.Search(text);
        }
        public async Task<List<ProductEntity>> GetProductByPrice(float maxLimit,float lowerLimit)
        {
            return await _productRepository.GetProductByPrice(maxLimit, lowerLimit);
        }
        public async Task<List<ProductEntity>> GetProductByRating(int minRating)
        {
            return await _productRepository.GetProductByRating(minRating);
        }
    }
}
