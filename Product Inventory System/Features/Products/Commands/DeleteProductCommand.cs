using MediatR;

namespace Product_Inventory_System.Features.Products.Commands
{
    public record DeleteProductCommand(int ProductId) : IRequest<bool>;
}
