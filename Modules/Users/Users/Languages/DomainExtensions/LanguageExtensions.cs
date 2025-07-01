using Users.Languages.Dtos;
using Users.Languages.Models;

namespace Users.Languages.DomainExtensions;

public static class LanguageExtensions
{
  public static LanguageDto ToDto(this Language language)
  {
    return new LanguageDto(
      language.Id,
      language.Name,
      language.Code,
      language.Description,
      language.IsDefault,
      language.IsActive,
      language.CreatedAt,
      language.UpdatedAt
    );
  }
  public static IQueryable<LanguageDto> ProjectListToDto(this IQueryable<Language> query)
  {
    return query.Select(language => new LanguageDto(
      language.Id,
      language.Name,
      language.Code,
      language.Description,
      language.IsDefault,
      language.IsActive,
      language.CreatedAt,
      language.UpdatedAt
    ));
  }


  public static IQueryable<Language> FilterBySearchTerm(this IQueryable<Language> query, string? searchTerm)
  {
    if (string.IsNullOrWhiteSpace(searchTerm))
      return query;

    var normalizedSearchTerm = searchTerm.Trim().ToLower();
    return query.Where(x =>
      x.Name.ToLower().Contains(normalizedSearchTerm) ||
      x.Code.ToLower().Contains(normalizedSearchTerm));
  }

  public static IQueryable<Language> FilterByIsActive(this IQueryable<Language> query, bool? isActive)
  {
    if (!isActive.HasValue)
      return query;

    return query.Where(x => x.IsActive == isActive.Value);
  }

  public static IQueryable<Language> FilterByIsDefault(this IQueryable<Language> query, bool? isDefault)
  {
    if (!isDefault.HasValue)
      return query;

    return query.Where(x => x.IsDefault == isDefault.Value);
  }

  public static IQueryable<Language> OrderByDefaultAndName(this IQueryable<Language> query)
  {
    return query.OrderByDescending(x => x.IsDefault)
               .ThenBy(x => x.Name);
  }

  public static async Task<Language?> GetDefaultLanguageAsync(this IQueryable<Language> query, CancellationToken cancellationToken = default)
  {
    return await query.FirstOrDefaultAsync(x => x.IsDefault && x.IsActive, cancellationToken);
  }

  public static async Task<bool> IsCodeUniqueAsync(this IQueryable<Language> query, string code, Guid? excludeId = null, CancellationToken cancellationToken = default)
  {
    var normalizedCode = code.ToUpperInvariant();
    return !await query.AnyAsync(x =>
      x.Code.ToUpperInvariant() == normalizedCode &&
      (!excludeId.HasValue || x.Id != excludeId.Value),
      cancellationToken);
  }
}