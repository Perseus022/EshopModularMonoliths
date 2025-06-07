using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest PaginationRequest)
    : IQuery<GetProductsResult>;

public record GetProductsResult(PaginatedResult<ProductDto> Products);

internal class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        // Retrieve all products from the database
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        // Calculate the total count of products for pagination
        var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);

        var products = await dbContext.Products
            .AsNoTracking() // Use AsNoTracking for read-only queries to improve performance
            .OrderBy(p => p.Name) // Order by product name
            .Skip(pageIndex * pageSize) // Skip the number of items based on page index
            .Take(pageSize) // Take the number of items based on page size
            .ToListAsync(cancellationToken);
        // Mapping product entities to product DTOs using Mapster
        var productDtos = products.Adapt<List<ProductDto>>();
        return new GetProductsResult(
            new PaginatedResult<ProductDto>(
                pageIndex,
                pageSize,
                totalCount,
                productDtos)
            );
    }
}
