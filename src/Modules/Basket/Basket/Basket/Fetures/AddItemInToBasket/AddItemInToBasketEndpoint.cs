
namespace Basket.Basket.Fetures.AddItemInToBasket;

public record AddItemInToBasketRequest(string userName, ShoppingCartItemDto ShoppingCartItem);

public record AddItemInToBasketResponse(Guid Id);

public class AddItemInToBasketEndpoint : ICarterModule
{
    void ICarterModule.AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{userName}/items", 
            async ([FromRoute] string userName, 
                   [FromBody] AddItemInToBasketRequest request, ISender sender) =>
        {
            var command = new AddItemInToBasketCommand(userName, request.ShoppingCartItem);
            var result = await sender.Send(command);
            var response = result.Adapt<AddItemInToBasketResponse>();
            return Results.Created($"/basket/{response.Id}", response);
        })
        .Produces<AddItemInToBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add Item to Basket")
        .WithDescription("Adds an item to a user's shopping basket by their username.")
        .RequireAuthorization();
    }
}
