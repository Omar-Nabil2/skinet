using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            var spec = new ProductSpecification(brand, type, sort);

            var products = await repo.ListAsync(spec);

            return Ok(products);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await repo.GetItemByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.Add(product);
            if(await repo.SaveChangesAsync())
            {
                return CreatedAtAction("GetProductById", new { id = product.Id }, product);
            }
            return BadRequest("Can't create the product");
        }
        bool ProductExists(int id)
        {
            return repo.Exists(id);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
                return BadRequest("Can't find the product");

            repo.Update(product);
            if (await repo.SaveChangesAsync())
                return NoContent();
            else
                return BadRequest("Something went wrong");
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repo.GetItemByIdAsync(id);

            if (product == null) return NotFound();

            repo.Delete(product);

            if (await repo.SaveChangesAsync())
                return NoContent();
            else
                return BadRequest("Something went wrong");
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<string>>> GetBrands()
        {
            var Spec = new BrandListSpecification();

            return Ok(await repo.ListAsync(Spec));
        }
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<string>>> GetTypes()
        {
            var Spec = new TypeListSpecification();

            return Ok(await repo.ListAsync(Spec));
        }
    }
}
