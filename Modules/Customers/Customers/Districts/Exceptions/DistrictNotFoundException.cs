using System;
using Shared.Exceptions;

namespace Customers.Districts.Exceptions;

public class DistrictNotFoundException : NotFoundException
{
  public DistrictNotFoundException(string message) : base(message)
  {
  }
}