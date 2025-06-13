using MediatR;
using Product_Inventory_System.Data;
using Product_Inventory_System.Features.Products.Commands;

namespace Product_Inventory_System.Features.Products.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly ProductDbContext _context;

        public CreateProductCommandHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Models.Product
            {
                Name = request.Name,
                Category = request.Category,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product.ProductId;
        }
    }
}
