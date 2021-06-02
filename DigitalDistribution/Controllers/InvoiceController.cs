using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Responses.Invoice;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Controllers
{
    [ApiController]
    [Route("api/invoices")]
    public class InvoiceController:ControllerBase
    {
        private readonly InvoiceService _invoiceService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;   

        public InvoiceController(InvoiceService invoiceService,
            UserService userService,
            IMapper mapper)
        {
            _invoiceService = invoiceService;
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ObjectResult>GetAllInvoices()
        {
            var normalUser =await _userService.Get(p => p.Id == User.GetUserId())
                .Include(u=>u.Bills)
                .FirstOrDefaultAsync();

            if (normalUser is null)
                return Ok(null);

            return Ok(_mapper.Map<List<InvoiceResponse>>(normalUser.Bills));
        }
        [HttpGet("{invoiceId}")]
        public async Task<ObjectResult>GetInvoicesById([FromRoute]int invoiceId)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                 .Include(u => u.Bills.Where(p=>p.Id==invoiceId))
                 .FirstOrDefaultAsync();

            if (normalUser?.Bills.FirstOrDefault() is null)
                return Ok(null);

            return Ok(_mapper.Map<InvoiceResponse>(normalUser.Bills.FirstOrDefault()));
        }
        
    }
}
