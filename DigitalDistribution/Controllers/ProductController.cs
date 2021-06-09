using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Constants;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.Product;
using DigitalDistribution.Models.Database.Requests.Update;
using DigitalDistribution.Models.Database.Responses.Product;
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
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly BaseService<UpdateEntity> _updateService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public ProductController(ProductService productService,
            BaseService<UpdateEntity> updateService,
            UserService userService,
            IMapper mapper)
        {
            _productService = productService;
            _updateService = updateService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ObjectResult> GetAllProducts()
        {
            var products = await _productService.Get()
                .Include(p=>p.DevTeam)
                .Include(p=>p.Updates)
                .Include(p=>p.Reviews)
                .ToListAsync();

            if (products is null)
                throw new NotFoundException(StringConstants.NoProductFound);
            
            return Ok(_mapper.Map<List<ProductResponse>>(products));
        }

        [HttpGet("{text}")]
        public async Task<ObjectResult> SearchProduct([FromRoute] string text)
        {
            var result = await _productService.Search(text);
            if (result is null)
                throw new NotFoundException(StringConstants.NoProductFound);

            return Ok(_mapper.Map<List<ProductResponse>>(result));
        }

        [HttpGet("price/{lowerLimit}&{upperLimit}")]
        public async Task<ObjectResult> GetProductByPrice([FromRoute] float lowerLimit,[FromRoute] float upperLimit = 0)
        {
            if (lowerLimit < 0 || lowerLimit > upperLimit)
                throw new BadRequestException(StringConstants.BadProductPriceEx);

            var products = await _productService.GetProductByPrice(upperLimit, lowerLimit);
            if (products is null)
                throw new NotFoundException(StringConstants.NoProductFound);

            return Ok(_mapper.Map<List<ProductResponse>>(products));
        }

        [HttpGet("rating/{minimalLimit}")]
        public async Task<ObjectResult> GetProductByRating([FromRoute] int minimalLimit)
        {
            if(minimalLimit<0 || minimalLimit>10)
                throw new BadRequestException(StringConstants.BadReviewRatingEx);

            var products = await _productService.GetProductByRating(minimalLimit);
            if (products is null)
                throw new NotFoundException(StringConstants.NoProductFound);

            return Ok(_mapper.Map<List<ProductResponse>>(products));
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<ObjectResult> NewProduct([FromBody] ProductEntity product)
        {
            return Ok(await _productService.Create(product));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{productId}")]
        public async Task<ObjectResult> DeleteProduct([FromRoute] int productId)
        {
            var result = await _productService.Get(p => p.Id == productId).FirstOrDefaultAsync();
            if (result is null)
                throw new NotFoundException(StringConstants.NoProductFound);

            return Ok(_productService.Delete(result));
        }

        [Authorize(Roles ="Admin")]
        [HttpPut("update/{productId}")]
        public async Task<ObjectResult>UpdateProduct([FromRoute]int productId,UpdateProductRequest productReq)
        {
            var product = await _productService.Get(p => p.Id == productId).FirstOrDefaultAsync();
            if (product is null)
                throw new NotFoundException(StringConstants.NoProductFound);

            return Ok(await _productService.Update(_mapper.Map(productReq, product)));
        }

        //updates

        [Authorize(Roles = "Developer")]
        [HttpPost("updates/{productId}")]
        public async Task<ObjectResult> AddUpdate([FromBody] UpdateEntity update, [FromRoute] int productId)
        {
            var devUser = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.DevTeam)
               .ThenInclude(p => p.Products.Where(u => u.Id == productId))
               .ThenInclude(p=>p.Updates)
               .FirstOrDefaultAsync();

            if (devUser?.DevTeam.Products.FirstOrDefault() is null)
                throw new NotFoundException(StringConstants.NoProductFound);

            update.ProductId = devUser.DevTeam.Products.First().Id;
            return Ok(await _updateService.Create(update)); 
           
        }

        [Authorize(Roles = "Developer")]
        [HttpPut("updates/{updateId}")]
        public async Task<ObjectResult> ChangeUpdate([FromRoute] int updateId,[FromBody] UpdateRequest update)
        {
            var devUser = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.DevTeam)
               .ThenInclude(p=>p.Products)
               .ThenInclude(p=>p.Updates.Where(u=>u.Id==updateId))
               .FirstOrDefaultAsync();

            if (devUser?.DevTeam.Products.FirstOrDefault().Updates.FirstOrDefault() is null)
                throw new NotFoundException(StringConstants.UpdateNotFound);

            return Ok(await _updateService.Update(_mapper.Map(update, devUser.DevTeam.Products.First().Updates.First())));
        }
    }
}
