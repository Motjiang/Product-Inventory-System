using MediatR;

namespace Product_Inventory_System.Features.Products.Commands
{
    public record UpdateProductCommand(int ProductId, string Name, string Category) : IRequest<bool>;
}
