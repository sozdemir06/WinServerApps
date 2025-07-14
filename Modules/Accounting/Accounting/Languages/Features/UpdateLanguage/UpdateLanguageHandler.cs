using Accounting.Data;
using Accounting.Languages.Dtos;
using Accounting.Languages.Models;

namespace Accounting.Languages.Features.UpdateLanguage;

public record UpdateLanguageCommand(LanguageDto Language) : ICommand<UpdateLanguageResult>;

public record UpdateLanguageResult(bool Success);

public class UpdateLanguageCommandValidator : AbstractValidator<UpdateLanguageCommand>
{
  public UpdateLanguageCommandValidator()
  {
    RuleFor(x => x.Language.Id).NotEmpty().WithMessage("Id is required");
    RuleFor(x => x.Language.Name).NotEmpty().WithMessage("Name is required");
    RuleFor(x => x.Language.Code).NotEmpty().WithMessage("Code is required");
  }
}

public class UpdateLanguageHandler(AccountingDbContext dbContext) : ICommandHandler<UpdateLanguageCommand, UpdateLanguageResult>
{
  public async Task<UpdateLanguageResult> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
  {
    var language = await dbContext.Languages
        .FirstOrDefaultAsync(x => x.Id == request.Language.Id || x.Code == request.Language.Code, cancellationToken);

    if (language == null)
    {
      // Create new language if not exists
      language = Language.Create(
          request.Language.Id,
          request.Language.Name,
          request.Language.Code,
          request.Language.Description,
          request.Language.IsDefault,
          request.Language.IsActive);

      await dbContext.Languages.AddAsync(language, cancellationToken);
    }
    else
    {
      // Update existing language
      language.Update(
          request.Language.Name,
          request.Language.Code,
          request.Language.Description,
          request.Language.IsDefault,
          request.Language.IsActive);
    }

    await dbContext.SaveChangesAsync(cancellationToken);
    return new UpdateLanguageResult(true);
  }
}