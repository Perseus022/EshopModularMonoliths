namespace Catalog.Products.Features.DeleteProduct;

//public record DeleteProductRequest(Guid Id);

public record DeleteProductResponse(bool Succes);

public class DeleteProductEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
           
            var result = await sender.Send(new DeleteProductCommand(id));
            var response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteProduct")
        .WithSummary("Delete a product")
        .WithDescription("Deletes a product from the catalog.")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest, "Invalid request data.")
        .ProducesProblem(StatusCodes.Status404NotFound, "Product not found.")
        .ProducesProblem(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
    }
}

