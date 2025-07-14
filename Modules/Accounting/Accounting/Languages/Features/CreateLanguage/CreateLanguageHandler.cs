using Accounting.Data;
using Accounting.Languages.Dtos;
using Accounting.Languages.Exceptions;
using Accounting.Languages.Models;

namespace Accounting.Languages.Features.CreateLanguage;

public record CreateLanguageCommand(LanguageDto Language) : ICommand<CreateLanguageResult>;

public record CreateLanguageResult(bool Success);

public class CreateLanguageCommandValidator : AbstractValidator<CreateLanguageCommand>
{
  public CreateLanguageCommandValidator()
  {
    RuleFor(x => x.Language.Name).NotEmpty().WithMessage("Name is required");
    RuleFor(x => x.Language.Code).NotEmpty().WithMessage("Code is required");
  }
}

public class CreateLanguageHandler(AccountingDbContext dbContext, ILocalizationService localizationService) : ICommandHandler<CreateLanguageCommand, CreateLanguageResult>
{
  public async Task<CreateLanguageResult> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
  {
    var existingLanguage = await dbContext.Languages
        .FirstOrDefaultAsync(x => x.Code == request.Language.Code, cancellationToken);

    if (existingLanguage != null)
    {
      throw new LanguageNotFoundException(await localizationService.Translate("AlreadyExistsMessage"));
    }

    var language = CreateNewLanguage(request.Language);
    await dbContext.Languages.AddAsync(language, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new CreateLanguageResult(true);
  }

  private Language CreateNewLanguage(LanguageDto languageDto)
  {
    var language = Language.Create(
        languageDto.Id,
        languageDto.Name,
        languageDto.Code,
        languageDto.Description,
        languageDto.IsDefault,
        languageDto.IsActive);

    return language;
  }
}