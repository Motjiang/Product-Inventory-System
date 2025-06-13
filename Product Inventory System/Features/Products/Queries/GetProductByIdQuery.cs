using MediatR;
using Product_Inventory_System.Models;

namespace Product_Inventory_System.Features.Products.Queries
{
    public record GetProductByIdQuery(int ProductId) : IRequest<Product>;
}
