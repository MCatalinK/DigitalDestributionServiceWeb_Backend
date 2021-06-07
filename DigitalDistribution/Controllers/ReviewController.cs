using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.Review;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController:ControllerBase
    {
        private readonly BaseService<ReviewEntity> _reviewService;
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly IMapper _mapper;

        public ReviewController(BaseService<ReviewEntity> reviewService,
            UserService userService,
            ProductService productService,
            IMapper mapper)
        {
            _reviewService = reviewService;
            _productService = productService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ObjectResult> GetAllReviewsWritten()
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p => p.Profile)
                .FirstOrDefaultAsync();

            if (normalUser?.Profile.Reviews is null)
                return Ok(null);//error

            return Ok(_reviewService.Get(p=>p.ProfileId==normalUser.Profile.Id));//reviews
        }

        [HttpGet("profile/{profileId}")]
        public async Task<ObjectResult> GetAllReviewsWrittenByProfile([FromQuery] int profileId)
        {
            var reviews = await _reviewService.Get(p => p.ProfileId == profileId).ToListAsync();

            if (reviews is null)
                return Ok(null);//error

            return Ok(reviews);//reviews
        }

        [HttpGet("product/{productId}")]
        public async Task<ObjectResult> GetAllReviewsWrittenForProduct([FromQuery] int productId)
        {
            var reviews = await _reviewService.Get(p => p.ProductId == productId).ToListAsync();

            if (reviews is null)
                return Ok(null);//error

            return Ok(reviews);//reviews
        }

        [HttpPost]
        public async Task<ObjectResult> AddReview([FromBody] ReviewEntity review,[FromQuery] int productId)
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p => p.Profile)
                .FirstOrDefaultAsync();

            var product = await _productService.Get(p => p.Id == productId).FirstOrDefaultAsync();

            if (product is null)
                return Ok(null);//error
            if (user?.Profile is null)
                return Ok(null);//error

            review.ProductId = product.Id;
            review.ProfileId = user.Profile.Id;
            int nrOfReviews = await _reviewService.Get(p => p.ProductId == product.Id).CountAsync();
            product.Rating = (product.Rating* nrOfReviews+review.Rating) / (nrOfReviews + 1);

            return Ok(await _reviewService.Create(review));
        }

        [HttpDelete("{reviewId}")]
        public async Task<ObjectResult> DeleteReview([FromQuery] int reviewId)
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.Profile)
               .ThenInclude(p => p.Reviews.Where(u => u.Id == reviewId))
               .FirstOrDefaultAsync();

            var review = user?.Profile.Reviews.FirstOrDefault();

            if (review is null)
                return Ok(null);//error

            var product = await _productService.Get(p => p.Id == review.Id).FirstOrDefaultAsync();

            int nrOfReviews = await _reviewService.Get(p => p.ProductId == product.Id).CountAsync();
            product.Rating = (product.Rating * nrOfReviews - review.Rating) / (nrOfReviews - 1);

            return Ok(_reviewService.Delete(user.Profile.Reviews.First()));
        }

        [HttpPut("{reviewId}")]
        public async Task<ObjectResult> UpdateReview ([FromQuery] int reviewId,[FromBody] UpdateReviewRequest update)
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.Profile)
               .ThenInclude(p => p.Reviews.Where(u=>u.Id==reviewId))
               .FirstOrDefaultAsync();

            var review = user?.Profile.Reviews.FirstOrDefault();

            if (review is null)
                return Ok(null);//error

            var product = await _productService.Get(p => p.Id == review.Id).FirstOrDefaultAsync();

            int nrOfReviews = await _reviewService.Get(p => p.ProductId == product.Id).CountAsync();
            product.Rating = (product.Rating * nrOfReviews - review.Rating + update.Rating) / (nrOfReviews);

            return Ok(await _reviewService.Update(_mapper.Map(update, review)));
        }
    }
}
