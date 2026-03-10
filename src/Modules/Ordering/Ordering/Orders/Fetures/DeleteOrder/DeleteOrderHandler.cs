using Ordering.Orders.Exceptions;

namespace Ordering.Orders.Fetures.DeleteOrder;

public record DeleteOrderCommand(Guid OrderId) 
    : ICommand<DeleteOrderResult>;
public record DeleteOrderResult(bool Success);

public class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderName is required");
    }
}

public class DeleteOrderHandler(OrderingDbContext dbContext)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.FindAsync([command.OrderId], cancellationToken: cancellationToken);
        if (order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }
        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteOrderResult(true);
    }
}
