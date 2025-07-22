using Shared.Contracts.CQRS;

namespace Catalog.Conracts.Products.Features.GetProductByID;

public record GetProductByIdQuery(Guid ProductId)
    : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(ProductDto Product);
