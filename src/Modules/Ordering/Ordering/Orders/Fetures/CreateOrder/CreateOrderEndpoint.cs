
namespace Ordering.Orders.Fetures.CreateOrder;

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);

internal class CreateOrderEndpoint : ICarterModule
{
    void ICarterModule.AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender, ClaimsPrincipal user) =>
        {
            var userName = user.Identity?.Name ?? "anonymous";

            var command = request.Adapt<CreateOrderCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<CreateOrderResponse>();

            return Results.Created($"/basket/{response.Id}", response);

        });
    }
}
