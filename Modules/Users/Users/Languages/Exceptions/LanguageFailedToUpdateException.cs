using Shared.Exceptions;

namespace Users.Languages.Exceptions;

public class LanguageFailedToUpdateException : BadRequestException
{
  public LanguageFailedToUpdateException(string languageId)
      : base($"Failed to update language with ID: {languageId}")
  {
  }
}