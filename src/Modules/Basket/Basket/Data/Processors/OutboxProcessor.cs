
using System.Text.Json;

namespace Basket.Data.Processors
{
    internal class OutboxProcessor 
        (IServiceProvider serviceProvider, IBus bus, ILogger<OutboxProcessor> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                { 
                    var scope = serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();

                    var outboxMessages = await dbContext.OutboxMessages
                        .Where(m => m.ProcessedOn == null)
                        .ToListAsync(stoppingToken);

                    foreach (var message in outboxMessages)
                    {
                        var eventType = Type.GetType(message.Type);
                        if (eventType == null)
                        {
                            logger.LogWarning("Event type {EventType} not found for OutboxMessage {MessageId}", message.Type, message.Id);
                            continue;
                        }

                        var eventMessage = JsonSerializer.Deserialize(message.Content, eventType);
                        if (eventMessage == null)
                        {
                            logger.LogWarning("Failed to deserialize Content for OutboxMessage {MessageId}", message.Id);
                            continue;
                        }

                        await bus.Publish(eventMessage, stoppingToken);

                        message.ProcessedOn = DateTime.UtcNow;

                        logger.LogInformation("Published event {EventType} from OutboxMessage {MessageId}", message.Type, message.Id);
                    }
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while processing outbox messages.");
                }

                   await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
