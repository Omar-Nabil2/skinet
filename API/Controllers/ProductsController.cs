using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductRepository productRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            return Ok(await productRepository.GetProductsAsync(brand, type, sort));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            productRepository.AddProduct(product);
            if(await productRepository.SaveChangesAsync())
            {
                return CreatedAtAction("GetProductById", new { id = product.Id }, product);
            }
            return BadRequest("Can't create the product");
        }
        bool ProductExists(int id)
        {
            return productRepository.ProductExists(id);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
                return BadRequest("Can't find the product");

            productRepository.UpdateProduct(product);
            if (await productRepository.SaveChangesAsync())
                return NoContent();
            else
                return BadRequest("Something went wrong");
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            productRepository.DeleteProduct(product);

            if (await productRepository.SaveChangesAsync())
                return NoContent();
            else
                return BadRequest("Something went wrong");
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<string>>> GetBrands()
        {
            return Ok(await productRepository.GetBrandsAsync());
        }
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<string>>> GetTypes()
        {
            return Ok(await productRepository.GetTypesAsync());
        }
    }
}
