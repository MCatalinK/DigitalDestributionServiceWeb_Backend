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
    public class BillingAddressService
    {
        private readonly BillingAddressRepository _addressRepository;
        protected ClaimsPrincipal CurrentUser;
        public BillingAddressService(BillingAddressRepository addressRepository,
            IHttpContextAccessor contextAccessor)
        {
            _addressRepository = addressRepository;
            CurrentUser = contextAccessor.HttpContext?.User;
        }
        public IQueryable<BillingAddressEntity> Get(Expression<Func<BillingAddressEntity, bool>> predicate = null)
        {
            return _addressRepository.Get(predicate);
        }

        public async Task Commit()
        {
            await _addressRepository.Commit();
        }

        public async Task<BillingAddressEntity> Create(BillingAddressEntity entity, bool commit = true)
        {
            return await _addressRepository.Create(entity, commit);
        }

        public async Task<BillingAddressEntity> Update(BillingAddressEntity entity, bool commit = true)
        {
            if (CurrentUser != null && CurrentUser.GetUserId() != 0)
            {
                entity.DateModified = DateTime.Now;
            }

            return await _addressRepository.Update(entity, commit);
        }
        public async Task<BillingAddressEntity> Delete(BillingAddressEntity entity, bool commit = true)
        {
            return await _addressRepository.Delete(entity, commit);
        }
    }
}
