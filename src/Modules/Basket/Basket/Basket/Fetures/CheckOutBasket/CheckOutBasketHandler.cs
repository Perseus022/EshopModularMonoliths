
using MassTransit;
using Shared.Messaging.Events;
using System.Text.Json;

namespace Basket.Basket.Fetures.CheckOutBasket;

public record CheckoutBasketCommand(BasketCheckOutDto BasketCheckout) 
    : ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckOutDto can't be null.");
        RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required.");
    }
}
internal class CheckOutBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        await using var transaction =
            await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // Get the basket details from the command
            var basket = await dbContext.ShoppingCarts
                .Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.UserName == command.BasketCheckout.UserName, cancellationToken);
            if (basket == null)
            {
                throw new BasketNotFoundException(command.BasketCheckout.UserName);
            }
            // Set the TotalPrice on BasketCheckout event message
            var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;
            // Write the BasketCheckout event message to OutboxMessages table in the same transaction
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(eventMessage),
                OccurredOn = DateTime.UtcNow
            };

            dbContext.OutboxMessages.Add(outboxMessage);
            // Delete the basket
            dbContext.ShoppingCarts.Remove(basket);

            await dbContext.SaveChangesAsync(cancellationToken);
            await dbContext.Database.CommitTransactionAsync(cancellationToken);

            return new CheckoutBasketResult(true);

        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return new CheckoutBasketResult(false);
        }

    }
    /// CheckoutBasket without using Outbox pattern, 
    /// directly publish the BasketCheckoutIntegrationEvent to RabbitMQ using MassTransit 
    /// and delete the basket after publishing the event. 
    /// This approach is simpler but may lead to data inconsistency 
    /// if the publish operation fails after deleting the basket. 
    /// Consider using Outbox pattern for better reliability in production scenarios.
    /// 
    //internal class CheckOutBasketHandler(IBasketRepository repository, IBus bus)
    //    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    //{
    //    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    //    {
    //        // Get the basket details from the command
    //        var basket = await repository.GetBasket(command.BasketCheckout.UserName, true, cancellationToken: cancellationToken);
    //        // Set the TotalPrice on BasketCheckout event message
    //        var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
    //        eventMessage.TotalPrice = basket.TotalPrice;
    //        // send the BasketCheckout event message to RabbitMQ using MassTransit
    //        await bus.Publish(eventMessage, cancellationToken);
    //        // Delete the basket
    //        await repository.DeleteBasket(command.BasketCheckout.UserName, cancellationToken);

    //        return new CheckoutBasketResult(true);
    //    }
    //}

}