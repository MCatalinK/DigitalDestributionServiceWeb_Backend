using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class ProductService : BaseService<ProductEntity>
    {
        private readonly ProductRepository _productRepository;
        public ProductService(ProductRepository productRepository,
            IHttpContextAccessor contextAccessor) 
            : base(productRepository, contextAccessor)
        {
            _productRepository = productRepository;
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
