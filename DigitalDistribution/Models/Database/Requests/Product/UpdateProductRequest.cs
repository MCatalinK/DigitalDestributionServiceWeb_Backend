using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Models.Database.Requests.Product
{
    public class UpdateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
    }
}
