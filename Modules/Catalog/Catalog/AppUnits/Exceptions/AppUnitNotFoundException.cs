using Shared.DDD;
using Shared.Exceptions;

namespace Catalog.AppUnits.Exceptions;

public class AppUnitNotFoundException : NotFoundException
{
  public AppUnitNotFoundException(string message, Guid id) : base(message)
  {
    Id = id;
  }

  public Guid Id { get; }
}