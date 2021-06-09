using DigitalDistribution.Models.Database.Entities;

namespace DigitalDistribution.Models.Database.Responses.Review
{
    public class ReviewResponseProfile:BaseResponse
    {
        public int Rating { get; set; }
        public string Content { get; set; }
        public ProfileEntity Profile { get; set; }

    }
}
