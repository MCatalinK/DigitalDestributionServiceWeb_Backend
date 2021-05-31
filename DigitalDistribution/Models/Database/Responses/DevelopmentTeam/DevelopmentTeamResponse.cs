using DigitalDistribution.Models.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Models.Database.Responses.DevelopmentTeam
{
    public class DevelopmentTeamResponse:BaseResponse
    {
        public string Name { get; set; }
        public List<ProductEntity> ProductsCreated { get; set; }
    }
}
