namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery
    : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<ProductDto> Products);

internal class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        // Retrieve all products from the database
        var products = await dbContext.Products
            .AsNoTracking() // Use AsNoTracking for read-only queries to improve performance
            .OrderBy(p => p.Name) // Order by product name
            .ToListAsync(cancellationToken);
        // Mapping product entities to product DTOs using Mapster
        var productDtos = products.Adapt<List<ProductDto>>();
        return new GetProductsResult(productDtos);
    }
}
