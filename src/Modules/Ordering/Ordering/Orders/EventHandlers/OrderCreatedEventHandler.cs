using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Orders.EventHandlers
{
    internal class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
        : INotificationHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            LoggerExtensions.LogInformation(logger, "Domain Event Handled {DomainEvent}",notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
