using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext storeContext;

        public ProductsController(StoreContext storeContext)
        {
            this.storeContext = storeContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await storeContext.products.ToListAsync();
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await storeContext.products.FindAsync(id);
            if (product == null) return NotFound();
            return product;
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            storeContext.products.Add(product);
            await storeContext.SaveChangesAsync();
            return product;
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id)) 
                return BadRequest("Can't find the product");

            storeContext.Entry(product).State = EntityState.Modified;
            await storeContext.SaveChangesAsync();
            return NoContent();
        }
        private bool ProductExists(int id)
        {
            return storeContext.products.Any(x => x.Id == id);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await storeContext.products.FindAsync(id);

            if (product == null) return NotFound();

            storeContext.Remove(product);

            await storeContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
