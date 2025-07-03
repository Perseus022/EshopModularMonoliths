
namespace Basket.Basket.Fetures.RemoveItemFromBasket;

// public record RemoveItemFromBasketRequest(atring UsereName, Guid ProductId)

public record RemoveItemFromBasketResponse(Guid Id);

public class RemoveItemFromBasketEndpoint : ICarterModule
{
    void ICarterModule.AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}/items/{productId}", async ([FromRoute] string userName, [FromRoute] Guid productId, ISender sender) =>
        {
            var command = new RemoveItemFromBasketCommand(userName, productId);
            var ressult = await sender.Send(command);
            var response = ressult.Adapt<RemoveItemFromBasketResponse>();
            return Results.Ok(response);
        })
        .Produces<RemoveItemFromBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Remove item from basket")
        .WithDescription("Removes an item from the user's shopping basket.");
    }
}
