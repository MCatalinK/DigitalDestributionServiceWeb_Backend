using DigitalDistribution.Models.Database.Entities;

namespace DigitalDistribution.Models.Database.Responses.Review
{
    public class ReviewResponseProduct:BaseResponse
    {
        public int Rating { get; set; }
        public string Content { get; set; }
        public ProductEntity Product { get; set; }

    }
}
