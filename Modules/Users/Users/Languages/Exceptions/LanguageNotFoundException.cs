using Shared.Exceptions;

namespace Users.Languages.Exceptions;

public class LanguageNotFoundException : NotFoundException
{
  public LanguageNotFoundException(string message) : base(message)
  {
  }

  public LanguageNotFoundException() : base("Language not found")
  {
  }
}