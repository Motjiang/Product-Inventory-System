using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Inventory_System.Data;
using Product_Inventory_System.Features.Products.Commands;
using Product_Inventory_System.Features.Products.Queries;

namespace Product_Inventory_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ProductDbContext _context;

        public ProductController(ISender sender, ProductDbContext context)
        {
            _sender = sender;
            _context = context;
        }

        [HttpPost("create-product")]
        public async Task<ActionResult<int>> CreateProduct([FromBody] CreateProductCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid product data.");
            }

            if (await CheckProductExistsAsync(command.Name, command.Category))
            {
                return Conflict("A product with the same name and category already exists.");
            }

            var productId = await _sender.Send(command);

            return productId;
        }

        [HttpGet("get-product/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _sender.Send(new GetProductByIdQuery(productId));

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(product);
        }

        [HttpGet("get-all-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _sender.Send(new GetAllProductsQuery());

            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }

        [HttpPut("update-product/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId,[FromBody] UpdateProductCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid product data.");
            }

            var product = new UpdateProductCommand(productId, command.Name, command.Category);

            if (await CheckProductExistsAsync(product.Name, product.Category, product.ProductId))
            {
                return Conflict("A product with the same name and category already exists.");
            }

            var result = await _sender.Send(product);
            if (!result)
            {
                return NotFound("Product not found or update failed.");
            }

            return Ok("Product updated successfully.");
        }

        [HttpDelete("delete-product/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var command = new DeleteProductCommand(productId);

            var result = await _sender.Send(command);

            if (!result)
            {
                return NotFound($"Product with ID {productId} not found.");
            }

            return Ok($"Product with ID {productId} has been deleted.");
        }

        #region Helpers
        private async Task<bool> CheckProductExistsAsync(string name, string category, int? productIdToExclude = null)
        {
            return await _context.Products
                .AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Category.ToLower() == category.ToLower() &&
                              (!productIdToExclude.HasValue || x.ProductId != productIdToExclude.Value));
        }
        #endregion
    }
}
