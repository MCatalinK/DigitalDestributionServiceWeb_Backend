using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class UpdateService : BaseService<UpdateEntity>
    {
        private readonly UpdateRepository _updateRepository;
        private readonly ProductRepository _productRepository;
        public UpdateService(UpdateRepository updateRepository
            , IHttpContextAccessor contextAccessor
            ,ProductRepository productRepository)
            : base(updateRepository, contextAccessor)
        {
            _updateRepository = updateRepository;
            _productRepository = productRepository;
        }

        public async Task<UpdateEntity> CreateUpdate(UpdateEntity entity, bool commit = true)
        {
            var product = await _productRepository.Get(p => p.Id == entity.ProductId).FirstOrDefaultAsync();
            product.Version = entity.Version;
            _ = await _productRepository.Update(product);
            return await _updateRepository.CreateUpdate(entity,commit);
        }
        public async Task<UpdateEntity> DeleteUpdate(UpdateEntity entity,bool commit=true)
        {
            _=await _updateRepository.Delete(entity, commit);
            var product = await _productRepository.Get(p=>p.Id==entity.ProductId).FirstOrDefaultAsync();
            var newEntity = await _updateRepository.Get().OrderBy(p=>p.Version).LastOrDefaultAsync();
            product.Version = newEntity.Version;
            _ = await _productRepository.Update(product);
            return entity;

        }
    }
}
