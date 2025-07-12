using Shared.Exceptions;

namespace Catalog.Languages.Exceptions;

public class LanguageNotFoundException : NotFoundException
{
  public LanguageNotFoundException(string message) : base(message) { }
}