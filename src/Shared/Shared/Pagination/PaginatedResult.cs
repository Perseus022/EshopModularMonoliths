namespace Shared.Pagination;

public  class PaginatedResult<TEntity>
    (int pageIndex, int pageSize, long Count, IEnumerable<TEntity> data)
    where TEntity : class
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public long TotalCount { get; } = Count;
    public IEnumerable<TEntity> Data { get; } = data;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
