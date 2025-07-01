

namespace Shared.Pagination;

public class PagedList<T> : List<T>
{
  public PaginationMetaData MetaData { get; set; }

  public PagedList(List<T> items, int count, int pageNumber, int pageSize)
  {
    MetaData = new PaginationMetaData
    {
      CurrentPage = pageNumber,
      PageSize = pageSize,
      TotalCount = count,
      TotalPages = (int)Math.Ceiling(count / (double)pageSize)
    };
    AddRange(items);
  }

  public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken)
  {
    var count = await source.CountAsync(cancellationToken);
    var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    return new PagedList<T>(items, count, pageNumber, pageSize);
  }



}