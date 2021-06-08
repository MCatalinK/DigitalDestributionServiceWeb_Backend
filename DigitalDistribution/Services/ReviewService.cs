using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class ReviewService:BaseService<ReviewEntity>
    {
        private readonly ReviewRepository _reviewRepository;
        public ReviewService(ReviewRepository reviewRepository,
            IHttpContextAccessor contextAccessor)
            : base(reviewRepository, contextAccessor)
        {
            _reviewRepository = reviewRepository;
        }

      
    }
}
