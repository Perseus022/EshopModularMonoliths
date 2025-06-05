namespace Catalog.Products.Features.DeleteProduct;

//public record DeleteProductRequest(Guid Id);

public record DeleteProductRespont(bool Succes);

public class DeleteProductEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id)); 
            var response = result.Adapt<DeleteProductResult>(); //DeleteProductRespont

            return Results.Ok(response);
        })
        .WithName("DeleteProduct")
        .WithSummary("Delete a product")
        .WithDescription("Deletes a product from the catalog.")
        .Produces<DeleteProductResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

