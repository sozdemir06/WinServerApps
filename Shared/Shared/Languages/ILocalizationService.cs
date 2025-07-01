namespace Shared.Languages;

public interface ILocalizationService
{
    Task<string> Translate(string key);
}
