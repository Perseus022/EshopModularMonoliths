namespace Ordering.Orders.Fetures.GetOrderById;

//public record GetOrderByIdRequest(Guid Id);
public record GetOrderByIdResponse(OrderDto Order);
public class GetOrdersByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetOrderByIdQuery(id));

            GetOrderByIdResponse response = result.Adapt<GetOrderByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetOrderById")
        .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get order by id")
        .WithDescription("Get order by id");
    }
}
