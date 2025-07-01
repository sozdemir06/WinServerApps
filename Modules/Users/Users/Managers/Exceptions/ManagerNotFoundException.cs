using Shared.Exceptions;

namespace Users.Managers.Exceptions;

public class ManagerNotFoundException : NotFoundException
{
  public ManagerNotFoundException(Guid id) : base($"Manager with ID '{id}' was not found.")
  {
  }

  public ManagerNotFoundException(string message) : base(message)
  {
  }
}