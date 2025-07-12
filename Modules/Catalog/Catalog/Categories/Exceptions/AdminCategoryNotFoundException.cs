using Shared.DDD;
using Shared.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

public class AdminCategoryNotFoundException : NotFoundException
{
  public AdminCategoryNotFoundException(string message, Guid id) : base(message)
  {
    Id = id;
  }

  public Guid Id { get; }
}