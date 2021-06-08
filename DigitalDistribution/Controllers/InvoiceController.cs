using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.Invoice;
using DigitalDistribution.Models.Database.Responses.Invoice;
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
        private readonly CheckoutItemService _itemService;
        private readonly ProductService _productService;
        private readonly UserService _userService;
        private readonly LibraryService _libraryService;
        private readonly IMapper _mapper;

        public InvoiceController(InvoiceService invoiceService,
            CheckoutItemService itemService,
            ProductService productService,
            UserService userService,
            LibraryService libraryService,
            IMapper mapper)
        {
            _invoiceService = invoiceService;
            _itemService = itemService;
            _productService = productService;
            _libraryService = libraryService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ObjectResult> GetAllInvoices()
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(u => u.Bills)
                .ThenInclude(u => u.CheckoutItems)
                .ThenInclude(u => u.Product)
                .FirstOrDefaultAsync();

            if (normalUser?.Bills is null)
                return Ok(null);

            return Ok(_mapper.Map<List<InvoiceResponse>>(normalUser.Bills));
        }

        [HttpGet("{lowerLimit}&{upperLimit}")]
        public async Task<ObjectResult> GetInvoicesByPrice([FromQuery] float minimalLimit = 0,[FromQuery] float maxLimit = 0)
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .FirstOrDefaultAsync();
            var bills = await _invoiceService.GetBillsByPrice(user, minimalLimit, maxLimit);
            return Ok(_mapper.Map<List<InvoiceResponse>>(bills));
        }

        [HttpGet("{invoiceId}")]
        public async Task<ObjectResult> GetInvoicesById([FromRoute] int invoiceId)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                 .Include(u => u.Bills.Where(p => p.Id == invoiceId && p.IsPayed == true))
                 .FirstOrDefaultAsync();

            if (normalUser?.Bills.FirstOrDefault() is null)
                return Ok(null);

            return Ok(_mapper.Map<InvoiceResponse>(normalUser.Bills.FirstOrDefault()));
        }

        [HttpPost]
        public async Task<ObjectResult> CreateNewInvoice([FromBody] InvoiceEntity invoice)
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p => p.Bills.Where(p => p.IsPayed == false))
                .FirstOrDefaultAsync();
            if (user?.Bills.FirstOrDefault() is null)
            {
                invoice.UserId = user.Id;
                return Ok(await _invoiceService.Create(invoice));
            }
            else
                return Ok(null);//error
        }

        [HttpPost("add/{productId}")]
        public async Task<ObjectResult> AddNewProduct([FromBody] CheckoutItemEntity itemEntity,[FromRoute] int productId)
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p => p.Bills.Where(p => p.IsPayed == false))
                .Include(p=>p.LibraryItems.Where(u=>u.ProductId==productId))
                .FirstOrDefaultAsync();

            var product = await _productService.Get(p => p.Id == productId)
                .FirstOrDefaultAsync();

            if (user?.Bills.FirstOrDefault() is null)
                return Ok(null);
            if (user.LibraryItems.FirstOrDefault() != null)
                return Ok(null);

            itemEntity.Licence = HelperExtensionMethods.CreateLicence();
            
            return Ok(await _itemService.AddItem(itemEntity));
        }

        [HttpDelete("delete/{productId}")]
        public async Task<ObjectResult> DeleteProduct([FromQuery] int productId)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(u => u.Bills.Where(p => p.IsPayed == false))
                .FirstOrDefaultAsync();

            var product = await _productService.Get(p => p.Id == productId)
                .FirstOrDefaultAsync();

            if (product is null)
                return Ok(null);

            if (normalUser?.Bills.FirstOrDefault() is null)
                return Ok(null);

            return Ok(await _itemService.Delete(normalUser.Bills.First(), product));
        }


        [HttpPut]
        public async Task<ObjectResult> PayInvoice()
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p => p.Bills.Where(u => u.IsPayed == false))
                .Include(p => p.LibraryItems)
                .FirstOrDefaultAsync();

            if (user?.Bills.FirstOrDefault() is null)
                return Ok(null);

            _ = await _libraryService.AddItem(user, user.Bills.First());

            return Ok(await _invoiceService.Update(_mapper.Map(new UpdateInvoiceRequest(),user.Bills.First())));
        }
    }
}

