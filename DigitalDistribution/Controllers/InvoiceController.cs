using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.Invoice;
using DigitalDistribution.Models.Database.Responses.Invoice;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/invoices")]
    public class InvoiceController:ControllerBase
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
        public async Task<ObjectResult>GetAllInvoices()
        {
            var normalUser =await _userService.Get(p => p.Id == User.GetUserId())
                .Include(u=>u.Bills)
                .ThenInclude(u=>u.CheckoutItems)
                .ThenInclude(u=>u.Product)
                .FirstOrDefaultAsync();

            if (normalUser?.Bills is null)
                return Ok(null);

            return Ok(_mapper.Map<List<InvoiceResponse>>(normalUser.Bills));
        }

        [HttpGet("{invoiceId}")]
        public async Task<ObjectResult>GetInvoicesById([FromRoute]int invoiceId)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                 .Include(u => u.Bills.Where(p=>p.Id==invoiceId && p.IsPayed==true))
                 .FirstOrDefaultAsync();

            if (normalUser?.Bills.FirstOrDefault() is null)
                return Ok(null);

            return Ok(_mapper.Map<InvoiceResponse>(normalUser.Bills.FirstOrDefault()));
        }

        [HttpPost("{productId}")]
        public async Task<ObjectResult>AddNewProduct([FromQuery] int productId)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                 .Include(u => u.Bills)
                 .FirstOrDefaultAsync();

            var product = await _productService.Get(p => p.Id == productId).FirstOrDefaultAsync();
            if (product is null)
                return Ok(null);

            if (normalUser is null)
                return Ok(null);


            var transaction = normalUser.Bills.Where(p => p.IsPayed == false).FirstOrDefault();
            if (transaction is null)
            {
                InvoiceEntity invoice = new InvoiceEntity
                {
                    Price = product.Price,
                    UserId = normalUser.Id
                };
                var item = await _itemService.AddItem(invoice, product);
                invoice.CheckoutItems.Add(item);
                return Ok(await _invoiceService.Create(invoice));
                //if it doesn't exist in library
            }
            else
            {
                var item = await _itemService.AddItem(transaction, product);
                transaction.CheckoutItems.Add(item);
                transaction.Price += product.Price;
                return Ok(await _invoiceService.Update(transaction));
            }
        }

        [HttpDelete("{productId}")]
        public async Task<ObjectResult> DeleteProduct([FromQuery] int productId)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(u => u.Bills)
                .FirstOrDefaultAsync();

            var product = await _productService.Get(p => p.Id == productId).FirstOrDefaultAsync();
            if (product is null)
                return Ok(null);

            if (normalUser is null)
                return Ok(null);

            var transaction = normalUser.Bills.Where(p => p.IsPayed == false).FirstOrDefault();
            if (transaction is null)
            {
                return Ok(null);//error
            }
            else
            {
                var item = await _itemService.Delete(transaction, product);
                transaction.Price -= product.Price;
                return Ok(await _invoiceService.Update(transaction));
            }
        }

        [HttpPut]
        public async Task<ObjectResult> PayInvoice()
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(u => u.Bills)
                .FirstOrDefaultAsync();

            if (normalUser is null)
                return Ok(null);

            var transaction = normalUser.Bills.Where(p => p.IsPayed == false).FirstOrDefault();
            if (transaction is null)
            {
                return Ok(null);//error
            }
            else
            {
                transaction.IsPayed = true;
                await _libraryService.AddItem(normalUser, transaction);
                return Ok(_invoiceService.Update(transaction));
            }
        }
    }
}
