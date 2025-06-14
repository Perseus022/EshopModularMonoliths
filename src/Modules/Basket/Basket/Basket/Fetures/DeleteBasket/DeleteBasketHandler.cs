
namespace Basket.Basket.Fetures.DeleteBasket;

public record DeleteBasketCommand(string UserNamee) 
    : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool Success);

internal class DeleteBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await dbContext.ShoppingCarts
            .SingleOrDefaultAsync(x => x.UserName == command.UserNamee, cancellationToken);
        if (basket == null)
        {
            throw new BasketNotFoundException(command.UserNamee);
        }
        dbContext.ShoppingCarts.Remove(basket);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteBasketResult(true);
    }
}
