using MassTransit;
using Microsoft.Extensions.Hosting;


namespace Users.Data.Processors;

public class OutboxProcessor(ILogger<OutboxProcessor> logger, IServiceProvider serviceProvider, IBus bus) : BackgroundService
{
  private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);
  private readonly int _maxRetryCount = 3;


  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        await ProcessOutboxMessagesAsync(stoppingToken);
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Error occurred while processing outbox messages");
      }

      await Task.Delay(_interval, stoppingToken);
    }
  }

  private async Task ProcessOutboxMessagesAsync(CancellationToken stoppingToken)
  {
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

    var messages = await dbContext.OutboxMessages
        .Where(x => x.ProcessedAt == null && x.RetryCount < _maxRetryCount)
        .OrderBy(x => x.CreatedAt)
        .Take(50)
        .ToListAsync(stoppingToken);

    try
    {

     
      foreach (var message in messages)
      {
        var eventType = Type.GetType(message.Type);
        if (eventType == null)
        {
          logger.LogWarning("Event type {EventType} not found", message.Type);
          continue;
        }

        var @event = JsonSerializer.Deserialize(message.Content, eventType);
        if (@event == null)
        {
          logger.LogWarning("Failed to deserialize event content for message {MessageId}", message.Id);
          continue;
        }

        await bus.Publish(@event, eventType, stoppingToken);

        message.ProcessedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(stoppingToken);

        logger.LogInformation("Successfully processed outbox message {MessageId}", message.Id);
      }
    }
    catch (Exception ex)
    {
      foreach (var message in messages)
      {
        message.RetryCount++;
        message.Error = ex.Message;
      }
      await dbContext.SaveChangesAsync(stoppingToken);

      logger.LogError(ex,
          "Error processing outbox messages. Batch size: {BatchSize}",
          messages.Count);
    }
  }
}