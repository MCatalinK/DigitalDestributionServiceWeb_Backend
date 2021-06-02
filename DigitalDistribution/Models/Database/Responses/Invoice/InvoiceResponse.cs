using DigitalDistribution.Models.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Models.Database.Responses.Invoice
{
    public class InvoiceResponse:BaseResponse
    {
        public float Price { get; set; }
        public List<ProductEntity> Products { get; set; }

    }
}
