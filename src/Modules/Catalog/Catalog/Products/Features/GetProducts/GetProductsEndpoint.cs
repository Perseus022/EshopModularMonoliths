using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

//public record GetProductsrequest(PaginationRequest, PaginationRequest);
public record GetProductsResponse(PaginatedResult<ProductDto> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ( [AsParameters] PaginationRequest request,ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery(request));
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .WithSummary("Retrieves all products from the catalog.")
        .WithDescription("This endpoint retrieves a list of all products available in the catalog.")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
