namespace Catalog.Products.Features.GetProducts;

//public record GetProductsrequest();
public record GetProductsResponse(IEnumerable<ProductDto> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery());
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .WithSummary("Retrieves all products from the catalog.")
        .WithDescription("This endpoint retrieves a list of all products available in the catalog.")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        .ProducesProblem(StatusCodes.Status200OK, "No products found.")
        .ProducesProblem(StatusCodes.Status400BadRequest, "Invalid request data.");
    }
}
