using System;

namespace DigitalDistribution.Models.Database.Responses.Profile
{
    public class ProfileDetailsResponse:BaseResponse
    {
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        
    }
}
