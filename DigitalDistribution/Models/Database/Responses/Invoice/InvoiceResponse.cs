using DigitalDistribution.Models.Database.Entities;
using System.Collections.Generic;

namespace DigitalDistribution.Models.Database.Responses.Invoice
{
    public class InvoiceResponse:BaseResponse
    {
        public float Price { get; set; }
        public List<CheckoutItemEntity> CheckoutItems { get; set; }
    }
}
