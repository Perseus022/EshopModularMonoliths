﻿namespace Catalog.Products.Features.GetProductByCategory;

public record GetProductByCategoryQuery(string Category)
    : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<ProductDto> Products);


internal class GetProductByCategoryHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        // Retrieve products by category from the database
        var products = await dbContext.Products
            .AsNoTracking() // Use AsNoTracking for read-only queries to improve performance
            .Where(p => p.Category.Contains(query.Category)) // Filter by category
            .OrderBy(p => p.Name) // Order by product name
            .ToListAsync(cancellationToken);
        // Mapping product entities to product DTOs using Mapster
        var productDtos = products.Adapt<List<ProductDto>>();
        return new GetProductByCategoryResult(productDtos);
    }

}
