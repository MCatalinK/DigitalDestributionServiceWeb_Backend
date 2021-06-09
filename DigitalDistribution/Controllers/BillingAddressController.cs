using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Constants;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.BillingAddress;
using DigitalDistribution.Models.Database.Responses.BillingAddress;
using DigitalDistribution.Models.Exceptions;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DigitalDistribution.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/addresses")]
    public class BillingAddressController : ControllerBase
    {
        private readonly BillingAddressService _billingAddressService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public BillingAddressController(BillingAddressService billingAddressService,
            UserService userService,
            IMapper mapper)
        {
            _billingAddressService = billingAddressService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ObjectResult> GetBillingAddressForUser()
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(a => a.Address)
                .Include(p=>p.Profile)
                .FirstOrDefaultAsync();

            if (user?.Address is null)
                throw new NotFoundException(StringConstants.BillingAddressNotFound);

            return Ok(_mapper.Map<BillingAddressResponse>(user.Address));
        }

        [HttpPost]
        public async Task<ObjectResult> AddBillingAddress([FromBody] BillingAddressEntity address)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
              .Include(p=>p.Address)
              .FirstOrDefaultAsync();

            if (normalUser?.Address is null)
            {
                address.UserId = normalUser.Id;
                return Ok(await _billingAddressService.Create(address));
            }
            throw new ItemExistsException(StringConstants.AddressAlreadyExists);
        }

        [HttpDelete]
        public async Task<ObjectResult> DeleteBillingAddress()
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(a => a.Address)
                .FirstOrDefaultAsync();

            if (user?.Address == null)
                throw new NotFoundException(StringConstants.BillingAddressNotFound);
            return Ok(await _billingAddressService.Delete(user.Address));
        }

        [HttpPut("Update")]
        public async Task<ObjectResult> UpdateBillingAddress([FromBody] UpdateBillingAddress address)
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(a => a.Address)
                .FirstOrDefaultAsync();

            if (user?.Address is null)
                throw new NotFoundException(StringConstants.BillingAddressNotFound);

            return Ok(await _billingAddressService.Update(_mapper.Map(address, user.Address)));
        }
    }
}
