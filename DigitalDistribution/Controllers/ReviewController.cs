using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Constants;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.Review;
using DigitalDistribution.Models.Database.Responses.Review;
using DigitalDistribution.Models.Exceptions;
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
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;
        private readonly UserService _userService;
        private readonly ProductService _productService;
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
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ObjectResult> GetReviews()
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p => p.Profile)
                .ThenInclude(p => p.Reviews)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync();

            if (user?.Profile.Reviews is null)
                throw new NotFoundException(StringConstants.NoReviewFound);

            return Ok(_mapper.Map<List<ReviewResponse>>(user.Profile.Reviews));
        }

        [HttpGet("{profileId}")]
        public async Task<ObjectResult> GetAllReviewsWrittenByProfile([FromRoute] int profileId)
        {
            var user = await _userService.Get()
                .Include(p => p.Profile)
                .ThenInclude(p => p.Reviews.Where(u => u.ProfileId == profileId))
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync();

            if (user?.Profile.Reviews is null)
                throw new NotFoundException(StringConstants.NoReviewFound);

            return Ok(_mapper.Map<List<ReviewResponseProduct>>(user.Profile.Reviews));
        }

        [HttpGet("get/{productId}")]
        public async Task<ObjectResult> GetAllReviewsWrittenForProduct([FromRoute] int productId)
        {
            var product = await _productService.Get()
                .Include(p => p.Reviews.Where(u => u.ProductId == productId))
                .ThenInclude(p => p.Profile)
                .FirstOrDefaultAsync();

            if (product?.Reviews is null)
                throw new NotFoundException(StringConstants.NoReviewFound); ;

            return Ok(_mapper.Map<List<ReviewResponseProfile>>(product.Reviews));
        }

        [Authorize(Roles = "User")]
        [HttpPost("{productId}")]
        public async Task<ObjectResult> AddReview([FromBody] ReviewEntity review,[FromRoute] int productId)
        {
            if (review.Rating > 10 || review.Rating < 0)
                throw new BadRequestException(StringConstants.BadReviewRatingEx);



            var product = await _productService.Get(p => p.Id == productId)
                .Include(p=>p.Reviews)
                .FirstOrDefaultAsync();

            var user = await _userService.Get(p => p.Id == User.GetUserId())               
                .Include(p => p.Profile)
                .ThenInclude(p=>p.Reviews.Where(u=>u.ProductId==product.Id))
                .Include(p => p.LibraryItems.Where(u => u.ProductId == productId))
                .FirstOrDefaultAsync();

            if (product is null)
                throw new NotFoundException(StringConstants.NoProductFound);

            if (user.Profile is null)
                throw new NotFoundException(StringConstants.ProfileNotFound);

            if (user.LibraryItems is null)
                throw new NotFoundException(StringConstants.LibraryItemNotFound);

            if (user?.Profile.Reviews.FirstOrDefault() is null)
            {
                review.ProductId = product.Id;
                review.ProfileId = user.Profile.Id;

                int nrOfReviews = await _reviewService.Get(p => p.ProductId == product.Id).CountAsync();
                product.Rating = (product.Rating* nrOfReviews+review.Rating) / (nrOfReviews + 1);
                _ = await _productService.Update(product);
                
                return Ok(await _reviewService.Create(review));
            }
            throw new ItemExistsException(StringConstants.ReviewExists);   
        }

        [Authorize(Roles = "User")]
        [HttpDelete("{productId}")]
        public async Task<ObjectResult> DeleteReview([FromRoute] int productId)
        {
            var user = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.Profile)
               .ThenInclude(p => p.Reviews.Where(u => u.ProductId==productId))
               .FirstOrDefaultAsync();
            var review = user?.Profile.Reviews.FirstOrDefault();

            if (review is null)
                throw new NotFoundException(StringConstants.NoReviewFound);

            var product = await _productService.Get(p => p.Id == review.ProductId).FirstOrDefaultAsync();
            int nrOfReviews = await _reviewService.Get(p => p.ProductId == product.Id).CountAsync();

            if (nrOfReviews > 1)
                product.Rating = (product.Rating * nrOfReviews - review.Rating) / (nrOfReviews - 1);
            else
                product.Rating = 0;

            _ = await _productService.Update(product);

            return Ok(await _reviewService.Delete(user.Profile.Reviews.First()));
        }

        [Authorize(Roles = "User")]
        [HttpPut("{reviewId}")]
        public async Task<ObjectResult> UpdateReview ([FromRoute] int reviewId,[FromBody] UpdateReviewRequest update)
        {
            if (update.Rating > 10 || update.Rating < 0)
                throw new BadRequestException(StringConstants.BadReviewRatingEx);

            var user = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.Profile)
               .ThenInclude(p => p.Reviews.Where(u=>u.Id==reviewId))
               .FirstOrDefaultAsync();

            var review = user?.Profile.Reviews.FirstOrDefault();

            if (user?.Profile.Reviews.FirstOrDefault() is null)
                throw new NotFoundException(StringConstants.NoReviewFound);

            var product = await _productService.Get(p => p.Id == review.ProductId).FirstOrDefaultAsync();

            int nrOfReviews = await _reviewService.Get(p => p.ProductId == product.Id).CountAsync();
            product.Rating = (product.Rating * nrOfReviews - review.Rating + update.Rating) / (nrOfReviews);
            _ = await _productService.Update(product);

            return Ok(await _reviewService.Update(_mapper.Map(update, user.Profile.Reviews.First())));
        }
    }
}
