namespace Users.Languages.Constants;

public static class LanguageConstants
{
  public const string DefaultLanguageCode = "tr-TR";
  public const string DefaultLanguageName = "Türkçe";

  public static class CacheKeys
  {
    public const string GetLanguageById = "language-{0}";
    public const string GetLanguages = "languages";
    public const string GetDefaultLanguage = "default-language";
  }
}