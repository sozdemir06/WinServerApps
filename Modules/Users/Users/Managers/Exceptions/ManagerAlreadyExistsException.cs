using Shared.Exceptions;

namespace Users.Managers.Exceptions;

public class ManagerAlreadyExistsException : BadRequestException
{
  public ManagerAlreadyExistsException(string field, string value)
      : base($"Manager with {field} '{value}' already exists.")
  {
  }

  public ManagerAlreadyExistsException(string message) : base(message)
  {
  }
}