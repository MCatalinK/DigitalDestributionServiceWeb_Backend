using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Constants;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.Invoice;
using DigitalDistribution.Models.Database.Responses.Invoice;
using DigitalDistribution.Models.Exceptions;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/invoices")]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;
        private readonly ProductService _productService;
        private readonly UserService _userService;
        private readonly LibraryService _libraryService;
        private readonly IMapper _mapper;

        public InvoiceController(InvoiceService invoiceService,
            ProductService productService,
            UserService userService,
            LibraryService libraryService,
            IMapper mapper)
        {
            _invoiceService = invoiceService;
            _productService = productService;
            _libraryService = libraryService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ObjectResult> GetAllInvoices()
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p=>p.Address)
                .ThenInclude(u => u.Bills)
                .ThenInclude(u => u.CheckoutItems)
                .ThenInclude(u => u.Product)
                .FirstOrDefaultAsync();

            if (normalUser?.Address is null)
                throw new NotFoundException(StringConstants.BillingAddressNotFound);

            if (normalUser?.Address.Bills is null)
                throw new NotFoundException(StringConstants.NoInvoicesFound);

            return Ok(_mapper.Map<List<InvoiceResponse>>(normalUser.Address.Bills));
        }

        [HttpGet("{lowerLimit}&{upperLimit}")]
        public async Task<ObjectResult> GetInvoicesByPrice([FromRoute] float lowerLimit = 0,[FromRoute] float upperLimit = 0)
        {
            if (lowerLimit < 0 || lowerLimit > upperLimit)
                throw new BadRequestException(StringConstants.BadProductPriceEx);

            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p=>p.Address)
                .ThenInclude(p=>p.Bills.Where(u=>u.Price>=lowerLimit && u.Price<=upperLimit && u.IsPayed==true))
                .ThenInclude(p=>p.CheckoutItems)
                .ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync();

            if (user?.Address.Bills.FirstOrDefault() is null)
                throw new NotFoundException(StringConstants.NoInvoicesFound);

            return Ok(_mapper.Map<List<InvoiceResponse>>(user.Address.Bills));
        }

        [HttpGet("{invoiceId}")]
        public async Task<ObjectResult> GetInvoicesById([FromRoute] int invoiceId)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p => p.Address)
                 .ThenInclude(u => u.Bills.Where(p => p.Id == invoiceId))
                 .ThenInclude(p=>p.CheckoutItems)
                 .ThenInclude(p => p.Product)
                 .FirstOrDefaultAsync();

            if (normalUser?.Address.Bills.FirstOrDefault() is null)
                throw new NotFoundException(StringConstants.NoInvoicesFound);

            return Ok(_mapper.Map<InvoiceResponse>(normalUser.Address.Bills.FirstOrDefault()));
        }
      
        [HttpDelete]
        public async Task<ObjectResult> DeleteCurrentInvoice()
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.Address)
               .ThenInclude(p => p.Bills.Where(p => p.IsPayed == false))
               .ThenInclude(p => p.CheckoutItems)
               .FirstOrDefaultAsync();
            if (user?.Address.Bills.FirstOrDefault() is null)
                throw new NotFoundException(StringConstants.NoInvoicesFound);
            
            return Ok(await _invoiceService.Delete(user.Address.Bills.First()));
        }
        
        [HttpPost("add/{productId}")]
        public async Task<ObjectResult> AddNewProduct([FromRoute] int productId)
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p=>p.Address)
                .ThenInclude(p => p.Bills.Where(p => p.IsPayed == false))
                .ThenInclude(p => p.CheckoutItems)
                .Include(p=>p.LibraryItems.Where(u=>u.ProductId==productId))
                .FirstOrDefaultAsync();

            var product = await _productService.Get(p => p.Id == productId)
                .Include(p => p.InvoiceItems)
                .FirstOrDefaultAsync();

            if (product is null)
                throw new NotFoundException(StringConstants.NoProductFound);

            if (user?.Address is null)
                throw new NotFoundException(StringConstants.BillingAddressNotFound);

            if (user?.Address.Bills.FirstOrDefault() is null)
            {
                InvoiceEntity invoice = new InvoiceEntity()
                {
                    AddressId = user.Address.Id
                };

                _=await _invoiceService.Create(invoice);
            }

            if (user.LibraryItems.FirstOrDefault() != null)
                throw new ItemExistsException(StringConstants.LibraryItemExists);


            CheckoutItemEntity itemEntity = new CheckoutItemEntity()
            {
                ProductId = product.Id,
                InvoiceId = user.Address.Bills.First().Id
            };
            if (user?.Address.Bills.FirstOrDefault().CheckoutItems != null
                && !user.Address.Bills.FirstOrDefault().CheckoutItems.Contains(itemEntity))
            {
                return Ok(await _invoiceService.AddItem(itemEntity));
                    
            }
            throw new ItemExistsException(StringConstants.InvoiceProductExists);

        }

        [HttpDelete("delete/{productId}")]
        public async Task<ObjectResult> DeleteProduct([FromRoute] int productId)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p=>p.Address)
                .ThenInclude(u => u.Bills.Where(p => p.IsPayed == false))
                .ThenInclude(p => p.CheckoutItems.Where(u=>u.ProductId==productId))
                .FirstOrDefaultAsync();

            var product = await _productService.Get(p => p.Id == productId)
                .FirstOrDefaultAsync();

            if (product is null)
                throw new NotFoundException(StringConstants.NoProductFound);

            if (normalUser?.Address.Bills.FirstOrDefault() is null)
                throw new NotFoundException(StringConstants.NoInvoicesFound);

            if (normalUser?.Address.Bills.First().CheckoutItems.FirstOrDefault() is null)
                throw new NotFoundException(StringConstants.ProductInvoiceNotFound);

            return Ok(await _invoiceService.RemoveItem(normalUser.Address.Bills.First().CheckoutItems.First()));
        }

        [HttpPut]
        public async Task<ObjectResult> PayInvoice()
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p=>p.Address)
                .ThenInclude(p => p.Bills.Where(u => u.IsPayed == false))
                .ThenInclude(p => p.CheckoutItems)
                .Include(p => p.LibraryItems)
                .FirstOrDefaultAsync();

            if (user?.Address is null)
                throw new NotFoundException(StringConstants.BillingAddressNotFound);

            if (user?.Address.Bills.FirstOrDefault() is null)
                throw new NotFoundException(StringConstants.NoInvoicesFound); ;

            _ = await _libraryService.AddItem(user, user.Address.Bills.First());

            return Ok(await _invoiceService.Update(_mapper.Map(new UpdateInvoiceRequest(),user.Address.Bills.First())));
        }
    } 
}

