using FastEndpoints;
using Shared.Services.Http;

namespace Users.TestEndpoints;

public record TestRequest(string? Message = null, int? Count = null);
public record TestResponse(string Message, DateTime Timestamp, int Count, bool Success);

public class TestEndpoint(ISender sender, IGetManagerRoleService getManagerRoleService, ILogger<TestEndpoint> logger)
    : Endpoint<TestRequest, TestResponse>
{
  public override void Configure()
  {
    Get("/test");
    Description(x => x
        .WithName("TestEndpoint")
        .WithTags("Test")
        .Produces<TestResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(TestRequest request, CancellationToken ct)
  {
    try
    {
      logger.LogInformation("Test endpoint called with message: {Message}, count: {Count}",
          request.Message, request.Count);

      var message = request.Message ?? "Hello from Test Endpoint!";
      var count = request.Count ?? 1;

      var response = new TestResponse(
          Message: message,
          Timestamp: DateTime.UtcNow,
          Count: count,
          Success: true
      );

      await getManagerRoleService.GetManagerRolesAsync(Guid.NewGuid(), ct);

      await SendAsync(response, StatusCodes.Status200OK, ct);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error occurred in test endpoint");
      await SendAsync(new TestResponse(
          Message: "Error occurred",
          Timestamp: DateTime.UtcNow,
          Count: 0,
          Success: false
      ), StatusCodes.Status500InternalServerError, ct);
    }
  }
}