using DigitalDistribution.Models.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Models.Database.Responses.Product
{
    public class ProductResponse:BaseResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public float Price { get; set; }
        public List<ReviewEntity> Reviews { get; set; }
    }
}
