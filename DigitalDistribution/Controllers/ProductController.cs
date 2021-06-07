using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.Product;
using DigitalDistribution.Models.Database.Responses.Product;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly UpdateService _updateService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public ProductController(ProductService productService,
            UpdateService updateService,
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
            var products = await _productService.Get().ToListAsync();
            if (products is null)
                return Ok(null);

            return Ok(_mapper.Map<ProductResponse>(products));
        }
        [HttpGet("{text}")]
        public async Task<ObjectResult> SearchProduct([FromRoute] string text)
        {
            var result = await _productService.Search(text);
            if (result is null)
                return Ok(null);
            return Ok(_mapper.Map<ProductResponse>(result));
        }

        [HttpGet("price/{upperLimit}_{lowerLimit}")]
        public async Task<ObjectResult> GetProductByPrice([FromRoute] float upperLimit,[FromRoute] float lowerLimit = 0)
        {
            var products = await _productService.Get(p => p.Price <= upperLimit && p.Price >= lowerLimit).ToListAsync();
            if (products is null)
                return Ok(null);

            return Ok(_mapper.Map<ProductResponse>(products));
        }

        [HttpGet("rating/{minimalLimit}")]
        public async Task<ObjectResult> GetProductByRating( [FromRoute] int minimalLimit)
        {
            var products = await _productService.Get(p => p.Rating >= minimalLimit).ToListAsync();
            if (products is null)
                return Ok(null);

            return Ok(_mapper.Map<ProductResponse>(products));
        }

        [HttpPost]
        public async Task<ObjectResult> NewProduct([FromBody] ProductEntity product)
        {
            return Ok(await _productService.Create(product));
        }

        [HttpDelete("{productId}")]
        public async Task<ObjectResult> DeleteProduct([FromRoute] int productId)
        {
            var result = await _productService.Get(p => p.Id == productId).FirstOrDefaultAsync();
            if (result is null)
                return Ok(null);
            return Ok(_productService.Delete(result));
        }

        [HttpPut("update/{productId}")]
        public async Task<ObjectResult>UpdateProduct([FromRoute]int productId,UpdateProductRequest productReq)
        {
            var product = await _productService.Get(p => p.Id == productId).FirstOrDefaultAsync();
            if (product is null)
                return Ok(null);
            return Ok(await _productService.Update(_mapper.Map(productReq, product)));
        }

        //updates

        [HttpPost("updates/{productId}")]
        public async Task<ObjectResult> AddUpdate([FromBody] UpdateEntity update, [FromRoute] int productId)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.DevTeam)
               .ThenInclude(p => p.Products.Where(u => u.Id == productId))
               .FirstOrDefaultAsync();

            if (normalUser?.DevTeam.Products.First() is null)
                return Ok(null);//exception

            update.ProductId = normalUser.DevTeam.Products.First().Id;
            return Ok(await _updateService.Create(update)); 
           
        }

        [HttpDelete("updates/{updateId},{productId}")]
        public async Task<ObjectResult> DeleteUpdate([FromRoute] int updateId, [FromRoute] int productId)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.DevTeam)
               .ThenInclude(p => p.Products.Where(u=>u.Id==productId))
               .FirstOrDefaultAsync();

            if (normalUser?.DevTeam.Products.First() is null)
                return Ok(null);//exception

            var result = await _productService.Search(productId);

            if (result is null)
                return Ok(null);

            var update = await _updateService.Get(p => p.Id == updateId).FirstOrDefaultAsync();
            return Ok(await _updateService.Delete(update));    
        }
    }
}
