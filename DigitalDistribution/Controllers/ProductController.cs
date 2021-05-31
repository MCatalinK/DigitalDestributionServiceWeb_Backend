using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.Product;
using DigitalDistribution.Models.Database.Responses.Product;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(ProductService productService,
            IMapper mapper)
        {
            _productService = productService;
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

        [HttpGet("Price/{upperLimit}{lowerLimit}")]
        public async Task<ObjectResult> GetProductByPrice([FromRoute] float upperLimit,[FromRoute] float lowerLimit = 0)
        {
            var products = await _productService.Get(p => p.Price <= upperLimit && p.Price >= lowerLimit).ToListAsync();
            if (products is null)
                return Ok(null);

            return Ok(_mapper.Map<ProductResponse>(products));
        }

        [HttpGet("Rating/{minimalLimit}")]
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
        public async Task<ObjectResult> DeleteProduct(int productId)
        {
            var result = await _productService.Get(p => p.Id == productId).FirstOrDefaultAsync();
            if (result is null)
                return Ok(null);
            return Ok(_productService.Delete(result));
        }

        [HttpPut("Update/{productId}")]
        public async Task<ObjectResult>UpdateProduct([FromRoute]int productId,UpdateProductRequest productReq)
        {
            var product = await _productService.Get(p => p.Id == productId).FirstOrDefaultAsync();
            if (product is null)
                return Ok(null);
            return Ok(await _productService.Update(_mapper.Map(productReq, product)));
        }
    }
}
