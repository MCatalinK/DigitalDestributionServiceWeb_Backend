using System;

namespace DigitalDistribution.Models.Database.Responses
{
    public class BaseResponse
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
