using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using Ordering.Orders.Fetures.CreateOrder;
using Shared.Messaging.Events;

namespace Ordering.Orders.EventHandlers;

public class BasketCheckoutIntegrationEventHandler(ISender sender, ILogger<BasketCheckoutIntegrationEventHandler> logger)
    : IConsumer<BasketCheckoutIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
    {
        var message = context.Message;
        // Here you can handle the event, for example, create an order based on the basket checkout information

        Console.WriteLine($"Received BasketCheckoutIntegrationEventHnadler:", context.Message.GetType().Name);
        // Create an order and start order fullfillment process, then publish an OrderCreatedIntegrationEvent or similar event to notify other services
        var createOrderCommand = MapToCreateOrderCommand(message);
        await sender.Send(createOrderCommand);

    }

    private IRequest<object> MapToCreateOrderCommand(BasketCheckoutIntegrationEvent message)
    {
        var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country, message.State, message.ZipCode);
        var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.Cvv, message.PaymentMethod);
        var orderId = Guid.NewGuid();

        var orderDto = new OrderDto(
            Id: orderId,
            CustomerId: message.CustomerId,
            OrderName: message.UserName, 
            ShippingAddress: addressDto,
            BillingAddress: addressDto, // Assuming billing and shipping address are the same for simplicity
            Payment: paymentDto,
            Items:
            [
                new OrderItemDto(orderId, new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"), 2, 500),
                new OrderItemDto(orderId, new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"), 1, 400)
            ]);

        return new CreateOrderCommand(orderDto);
    }
}
