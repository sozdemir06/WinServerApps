using Users.Languages.Constants;
using Users.Languages.Dtos;
using Users.Languages.Models;

namespace Users.Languages.Features.CreateLanguage;

public record CreateLanguageCommand( LanguageDto Language) : ICommand<CreateLanguageResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Languages];
}

public record CreateLanguageResult(Guid Id);

public class CreateLanguageCommandValidator : AbstractValidator<CreateLanguageCommand>
{
  public CreateLanguageCommandValidator()
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

    RuleFor(x => x.Language.Description)
        .MaximumLength(500)
        .WithMessage("Language description cannot exceed 500 characters");
  }
}

public class CreateLanguageHandler : ICommandHandler<CreateLanguageCommand, CreateLanguageResult>
{
  private readonly UserDbContext _dbContext;
  private readonly ILogger<CreateLanguageHandler> _logger;

  public CreateLanguageHandler(UserDbContext dbContext, ILogger<CreateLanguageHandler> logger)
  {
    _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public async Task<CreateLanguageResult> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
  {
    var existingLanguage = await _dbContext.Languages
        .FirstOrDefaultAsync(x => x.Code.ToLower() == request.Language.Code.ToLower(), cancellationToken);

    if (existingLanguage is not null)
    {
      throw new InvalidOperationException($"Language with code {request.Language.Code} already exists");
    }

    var language = Language.Create(
        request.Language.Name,
        request.Language.Code,
        request.Language.Description,
        request.Language.IsDefault,
        request.Language.IsActive
    );

    await _dbContext.Languages.AddAsync(language, cancellationToken);
    await _dbContext.SaveChangesAsync(cancellationToken);

    return new CreateLanguageResult(language.Id);
  }
}