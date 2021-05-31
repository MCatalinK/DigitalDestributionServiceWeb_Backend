using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;

namespace DigitalDistribution.Services
{
    public class BillingAddressService : BaseService<BillingAddressEntity>
    {
        public BillingAddressService(BillingAddressRepository billingAddressRepository,
            IHttpContextAccessor contextAccessor)
            : base(billingAddressRepository, contextAccessor)
        {
        }
    }
}
