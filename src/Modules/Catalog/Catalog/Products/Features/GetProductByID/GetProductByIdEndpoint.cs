namespace Catalog.Products.Features.GetProductByID;

public record GetproductByIdResponse(ProductDto Product);
public class GetProductByIdEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));
            var response = result.Adapt<GetproductByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .WithSummary("Retrieves a product by its ID.")
        .WithDescription("This endpoint retrieves a specific product from the catalog using its unique identifier.")
        .Produces<GetproductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
