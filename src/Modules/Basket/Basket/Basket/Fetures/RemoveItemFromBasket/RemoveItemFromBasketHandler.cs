namespace Basket.Basket.Fetures.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(string UserNamee, Guid ProductId)
    : ICommand<RemoveItemFromBasketResult>;
public record RemoveItemFromBasketResult(Guid Id);

public class RemoveItemFromBasketValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketValidator()
    {
        RuleFor(x => x.UserNamee).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
    }
}


internal class RemoveItemFromBasketHandler(IBasketRepository repository)
    : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await repository.GetBasket(command.UserNamee, false, cancellationToken);

        shoppingCart.RemoveItem(command.ProductId);

        await repository.SaveChangesAsync(cancellationToken);

        return new RemoveItemFromBasketResult(shoppingCart.Id);
    }
}
