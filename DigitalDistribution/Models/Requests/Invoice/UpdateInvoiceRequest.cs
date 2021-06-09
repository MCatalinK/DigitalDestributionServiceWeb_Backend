using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Models.Database.Requests.Invoice
{
    public class UpdateInvoiceRequest
    {
        public bool IsPayed { get; set; } = true;
    }
}
