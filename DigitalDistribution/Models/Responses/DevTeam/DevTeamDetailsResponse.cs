using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Responses;
using System.Collections.Generic;

namespace DigitalDistribution.Models.Responses.DevTeam
{
    public class DevTeamDetailsResponse:BaseResponse
    {
        public string Name { get; set; }
        public List<ProductEntity> Products { get; set; }
    }
}
