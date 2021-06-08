using DigitalDistribution.Models.Database.Entities;
using System.Collections.Generic;

namespace DigitalDistribution.Models.Database.Responses.Product
{
    public class ProductResponse:BaseResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
        public DevelopmentTeamEntity DevTeam { get; set; }
        public List<ReviewEntity> Reviews { get; set; }
    }
}
