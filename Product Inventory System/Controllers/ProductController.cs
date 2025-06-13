using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Inventory_System.Data;
using Product_Inventory_System.Features.Products.Commands;

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
