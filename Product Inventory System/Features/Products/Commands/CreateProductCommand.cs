using MediatR;

namespace Product_Inventory_System.Features.Products.Commands
{
    public record CreateProductCommand(string Name, string Category) : IRequest<int>;  
}
