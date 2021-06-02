using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class InvoiceService : BaseService<InvoiceEntity>
    {
        private readonly InvoiceRepository _invoiceRepository;
        public InvoiceService(InvoiceRepository invoiceRepository,
            IHttpContextAccessor contextAccessor) 
            : base(invoiceRepository, contextAccessor)
        {
            _invoiceRepository = invoiceRepository;
        }

    }
}
