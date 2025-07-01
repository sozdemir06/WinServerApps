namespace Shared.Pagination;

/// <summary>
/// Contains metadata information about a paginated result set
/// </summary>
public class PaginationMetaData
{
  /// <summary>
  /// Gets the current page number
  /// </summary>
  public int CurrentPage { get; set; }

  /// <summary>
  /// Gets the total number of pages
  /// </summary>
  public int TotalPages { get; set; }

  /// <summary>
  /// Gets the page size
  /// </summary>
  public int PageSize { get; set; }

  /// <summary>
  /// Gets the total number of items
  /// </summary>
  public int TotalCount { get; set; }




}