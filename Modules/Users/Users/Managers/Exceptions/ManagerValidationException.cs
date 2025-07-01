using System;
using Shared.Exceptions;

namespace Users.Managers.Exceptions;

public class ManagerValidationException : BadRequestException
{
  public ManagerValidationException(string message) : base(message)
  {
  }

  public ManagerValidationException(string message, string details) : base(message, details)
  {
  }
}