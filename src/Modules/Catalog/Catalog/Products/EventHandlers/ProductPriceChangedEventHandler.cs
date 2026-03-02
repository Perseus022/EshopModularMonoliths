using MassTransit;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHandlers;

public class ProductPriceChangedEventHandler
    (IBus bus, ILogger<ProductPriceChangedEventHandler> _logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event Handled: {DomainEvent}", notification.GetType().Name);

        //Publish product price changed integration event for upde baskeet price
        var integrationEvent = new ProductPriceChangedIntegrationEvent
        {
            ProductId = notification.Product.Id,
            Name = notification.Product.Name,
            Category = notification.Product.Category,
            Description = notification.Product.Description,
            ImageFile = notification.Product.ImageFile,
            Price = notification.Product.Price //Updated product price
        };

        await bus.Publish(integrationEvent, cancellationToken);
    }
}
