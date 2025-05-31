namespace Catalog.Products.EventHandlers;

public class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> _logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        //TODO: Publish product price changed integration event for upde baskeet price
        _logger.LogInformation("Domain Event Handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
