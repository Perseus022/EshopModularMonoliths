
namespace Ordering.Orders.Fetures.GetOrders;

public record GetOrdersQuery(PaginationRequest PeginationRequest) 
    : IQuery<GetOrdersResult>;

public record GetOrdersResult(PaginatedResult<OrderDto> Orders);

internal class GetOrdersHandler(OrderingDbContext dbContext)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PeginationRequest.PageIndex;
        var pageSize = query.PeginationRequest.PageSize;

        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

        var orders = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .OrderBy(p => p.OrderName)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var ordersDtos = orders.Adapt<List<OrderDto>>();

        return new GetOrdersResult(
        new PaginatedResult<OrderDto>(
            pageIndex,
            pageSize,
            totalCount,
            ordersDtos));
    }
}
