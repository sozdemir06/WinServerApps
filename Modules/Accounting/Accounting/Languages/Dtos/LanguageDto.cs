namespace Accounting.Languages.Dtos;

public record LanguageDto(
  Guid Id,
  string Name,
  string Code,
  string? Description,
  bool IsDefault,
  bool IsActive,
  DateTime CreatedAt,
  DateTime? UpdatedAt
  );