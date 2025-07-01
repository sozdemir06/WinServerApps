namespace Shared.Pagination;

/// <summary>
/// Base class for pagination parameters that can be used across the application
/// </summary>
public record PaginationParams
{
  private const int MaxPageSize = 50;


  public int PageNumber { get; set; } = 1;
  private int _pageSize = 10;


  public int PageSize
  {
    get => _pageSize;
    set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
  }


}