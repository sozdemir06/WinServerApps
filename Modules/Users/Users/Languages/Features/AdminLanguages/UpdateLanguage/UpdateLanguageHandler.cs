using Users.Languages.Dtos;
using Users.Languages.Exceptions;

namespace Users.Languages.Features.UpdateLanguage;

public record UpdateLanguageCommand(Guid Id, LanguageDto Language) : ICommand<UpdateLanguageResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Languages];
}

public record UpdateLanguageResult(bool Success);

public class UpdateLanguageCommandValidator : AbstractValidator<UpdateLanguageCommand>
{
  public UpdateLanguageCommandValidator()
  {
    RuleFor(x => x.Language.Name)
        .NotEmpty()
        .WithMessage("Language name is required")
        .MaximumLength(100)
        .WithMessage("Language name cannot exceed 100 characters");

    RuleFor(x => x.Language.Code)
        .NotEmpty()
        .WithMessage("Language code is required")
        .MaximumLength(10)
        .WithMessage("Language code cannot exceed 10 characters");
  }
}

public class UpdateLanguageHandler : ICommandHandler<UpdateLanguageCommand, UpdateLanguageResult>
{
  private readonly UserDbContext _dbContext;
  private readonly ILogger<UpdateLanguageHandler> _logger;

  public UpdateLanguageHandler(UserDbContext dbContext, ILogger<UpdateLanguageHandler> logger)
  {
    _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public async Task<UpdateLanguageResult> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
  {
    var language = await _dbContext.Languages.FindAsync([request.Id], cancellationToken)
        ?? throw new LanguageNotFoundException(request.Id.ToString());

    await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      // If the language is being set as default, ensure all other languages are set to non-default
      if (request.Language.IsDefault && !language.IsDefault)
      {
        var otherLanguages = await _dbContext.Languages
            .Where(l => l.Id != request.Id && l.IsDefault)
            .ToListAsync(cancellationToken);

        foreach (var otherLanguage in otherLanguages)
        {
          otherLanguage.Update(
              otherLanguage.Name,
              otherLanguage.Code,
              otherLanguage.Description,
              false, // Set IsDefault to false
              otherLanguage.IsActive
          );
        }

        _logger.LogInformation(
            "Set {Count} other languages to non-default when setting language {LanguageId} as default",
            otherLanguages.Count,
            request.Id);
      }

      language.Update(
          request.Language.Name,
          request.Language.Code,
          request.Language.Description,
          request.Language.IsDefault,
          request.Language.IsActive
      );

      await _dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new UpdateLanguageResult(true);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex,
          "Failed to update language. LanguageId: {LanguageId}, Name: {LanguageName}",
          request.Id,
          request.Language.Name
      );
      await transaction.RollbackAsync(cancellationToken);
      throw new LanguageFailedToUpdateException(request.Language.Id.ToString());
    }
  }


}