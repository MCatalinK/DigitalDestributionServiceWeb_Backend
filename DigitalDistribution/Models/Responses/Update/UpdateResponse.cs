using DigitalDistribution.Models.Database.Responses;

namespace DigitalDistribution.Models.Responses.Update
{
    public class UpdateResponse:BaseResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Version { get; set; }
    }
}
