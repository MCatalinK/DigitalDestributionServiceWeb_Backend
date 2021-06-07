using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Models.Database.Requests.Review
{
    public class UpdateReviewRequest
    {
        public int Rating { get; set; }
        public string Content { get; set; }
    }
}
