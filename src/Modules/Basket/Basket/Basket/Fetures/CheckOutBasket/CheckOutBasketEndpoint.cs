namespace Basket.Basket.Fetures.CheckOutBasket;

public record CheckoutBasketRequest(BasketCheckOutDto BasketCheckout);
public record CheckoutBasketResponse(bool IsSuccess);

public class CheckOutBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout",
            async (CheckoutBasketRequest request, ISender sender) =>
            {
                // Here you would typically handle the checkout logic, such as processing payment,

                var command = request.Adapt<CheckoutBasketCommand>();

                var result = await sender.Send(command);
                // updating inventory, and clearing the basket. For this example, we'll just return a success response.
                var response = result.Adapt<CheckoutBasketResponse>();

                return Results.Ok(response);
            })
        .WithName("CheckoutBasket")
        .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Checkout the basket for a user")
        .WithDescription("This endpoint processes the checkout of a user's basket, including payment and order creation.")
        .RequireAuthorization();
    }
}
