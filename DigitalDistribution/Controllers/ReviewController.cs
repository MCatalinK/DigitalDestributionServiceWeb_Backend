using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.Review;
using DigitalDistribution.Models.Database.Responses.Review;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ReviewService _reviewService;
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly ProfileService _profileService;
        private readonly IMapper _mapper;

        public ReviewController(ReviewService reviewService,
            UserService userService,
            ProductService productService,
            ProfileService profileService,
            IMapper mapper)
        {
            _reviewService = reviewService;
            _productService = productService;
            _userService = userService;
            _profileService = profileService;
            _mapper = mapper;
        }


        [HttpGet("{profileId}")]
        public async Task<ObjectResult> GetAllReviewsWrittenByProfile([FromRoute] int profileId)
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p => p.Profile)
                .FirstOrDefaultAsync();

            if (user?.Profile is null)
                return Ok(null);

            var result = await _reviewService.Get(p=>p.ProfileId==user.Profile.Id).ToListAsync();
            return Ok(_mapper.Map<List<ReviewResponse>>(result));
        }

        [HttpGet("get/{productId}")]
        public async Task<ObjectResult> GetAllReviewsWrittenForProduct([FromRoute] int productId)
        {
            var product = await _productService.Get(p => p.Id == productId).FirstOrDefaultAsync();
            if (product is null)
                return Ok(null);
            var result = await _reviewService.Get(p => p.ProductId == productId).ToListAsync();
            return Ok(_mapper.Map<List<ReviewResponse>>(result));
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ObjectResult> AddReview([FromBody] ReviewEntity review)
        {
            var product = await _productService.Get(p => p.Id == review.ProductId)
                .Include(p=>p.Reviews)
                .FirstOrDefaultAsync();

            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p => p.Profile)
                .ThenInclude(p=>p.Reviews.Where(u=>u.ProductId==product.Id))
                .FirstOrDefaultAsync();

            if (product is null)
                return Ok(null);

            if (user?.Profile.Reviews.FirstOrDefault() is null)
            {
                review.ProfileId = user.Profile.Id;
                int nrOfReviews = await _reviewService.Get(p => p.ProductId == product.Id).CountAsync();
                product.Rating = (product.Rating* nrOfReviews+review.Rating) / (nrOfReviews + 1);

                _ = await _productService.Update(product);

                return Ok(await _reviewService.Create(review));
            }
            return Ok(null);   
        }

        [Authorize(Roles = "User")]
        [HttpDelete("{reviewId}")]
        public async Task<ObjectResult> DeleteReview([FromRoute] int reviewId)
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
            if (nrOfReviews > 1)
                product.Rating = (product.Rating * nrOfReviews - review.Rating) / (nrOfReviews - 1);
            else
                product.Rating = 0;
            _ = await _productService.Update(product);

            return Ok(_reviewService.Delete(user.Profile.Reviews.First()));
        }

        [Authorize(Roles = "User")]
        [HttpPut("{reviewId}")]
        public async Task<ObjectResult> UpdateReview ([FromRoute] int reviewId,[FromBody] UpdateReviewRequest update)
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.Profile)
               .ThenInclude(p => p.Reviews.Where(u=>u.Id==reviewId))
               .FirstOrDefaultAsync();

            var review = user?.Profile.Reviews.FirstOrDefault();

            if (user?.Profile.Reviews.FirstOrDefault() is null)
                return Ok(null);//error

            var product = await _productService.Get(p => p.Id == review.Id).FirstOrDefaultAsync();

            int nrOfReviews = await _reviewService.Get(p => p.ProductId == product.Id).CountAsync();
            product.Rating = (product.Rating * nrOfReviews - review.Rating + update.Rating) / (nrOfReviews);
            _ = await _productService.Update(product);

            return Ok(await _reviewService.Update(_mapper.Map(update, review)));
        }
    }
}
