using MediatR;
using Microsoft.EntityFrameworkCore;
using Product_Inventory_System.Data;
using Product_Inventory_System.Features.Products.Queries;
using Product_Inventory_System.Models;

namespace Product_Inventory_System.Features.Products.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly ProductDbContext _context;

        public GetProductByIdQueryHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == request.ProductId, cancellationToken);

            return product;
        }
    }
}
