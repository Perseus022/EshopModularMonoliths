namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductRequest(ProductDto Product);
public record UpdateProductResponse(bool Success);

public class UpdateProductEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateProductResponse>();
            return Results.Ok(response);
        })
        .WithName("UpdateProduct")
        .WithSummary("Update an existing product")
        .WithDescription("Updates the details of an existing product in the catalog.")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest, "Invalid request data.")
        .ProducesProblem(StatusCodes.Status404NotFound, "Product not found.")
        .ProducesProblem(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
    }
}
