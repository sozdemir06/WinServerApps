using Shared.Exceptions;

namespace Accounting.Languages.Exceptions;

public class LanguageNotFoundException : NotFoundException
{
  public LanguageNotFoundException(string message) : base(message) { }
}