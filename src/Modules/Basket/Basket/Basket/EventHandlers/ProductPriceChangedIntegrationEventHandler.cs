using Basket.Basket.Fetures.UpdateItemPriceInBasket;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Basket.Basket.EventHandlers;

public class ProductPriceChangedIntegrationEventHandler
    (ISender sender, ILogger<ProductPriceChangedIntegrationEventHandler> _logger)
    : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        _logger.LogInformation("Integration Event Handled: {IntegrationEvent}", context.Message.GetType().Name);

        //Find the basket items that contain the updated product and update their price accordingly
        var command = new UpdateItemPriceInBasketCommand(context.Message.ProductId, context.Message.Price);
        //Mediator or direct repository access to update the basket items with the new price
        var result = await sender.Send(command);
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to update item price in basket for ProductId: {ProductId}", context.Message.ProductId);
        }

        _logger.LogInformation("Successfully updated item price in basket for ProductId: {ProductId}", context.Message.ProductId);

    }
}
