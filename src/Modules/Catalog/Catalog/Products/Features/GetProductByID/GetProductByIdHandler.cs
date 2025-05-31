namespace Catalog.Products.Features.GetProductByID;

public record GetProductByIdQuery(Guid ProductId)
    : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(ProductDto Product);

internal class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        // Retrieve the product by ID from the database
        var product = await dbContext.Products
            .AsNoTracking() // Use AsNoTracking for read-only queries to improve performance
            .SingleOrDefaultAsync(p => p.Id == query.ProductId, cancellationToken);

        if (product is null)
        {
            throw new Exception($"Product not found: {query.ProductId}"); // Product not found
        }
        // Mapping product entity to product DTO using Mapster
        var productDto = product.Adapt<ProductDto>();
        return new GetProductByIdResult(productDto);
    }
}
