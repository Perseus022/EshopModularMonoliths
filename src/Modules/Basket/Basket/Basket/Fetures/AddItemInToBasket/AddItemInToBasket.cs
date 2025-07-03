
namespace Basket.Basket.Fetures.AddItemInToBasket;

public record AddItemInToBasketCommand(string userName,ShoppingCartItemDto ShoppingCartItem)
    : ICommand<AddItemInToBasketResult>;

public record AddItemInToBasketResult(Guid Id);

public class AddItemInToBaskeetValidator : AbstractValidator<AddItemInToBasketCommand>
{
    public AddItemInToBaskeetValidator()
    {
        RuleFor(x => x.userName).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("Product ID is required.");
        RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}

internal class AddItemInToBasket(IBasketRepository repository)
    : ICommandHandler<AddItemInToBasketCommand, AddItemInToBasketResult>
{
    public async Task<AddItemInToBasketResult> Handle(AddItemInToBasketCommand command, CancellationToken cancellationToken)
    {

        var shoppingCart = await repository.GetBasket(command.userName,false, cancellationToken);

        shoppingCart.AddItem(command.ShoppingCartItem.ProductId, 
            command.ShoppingCartItem.Quantity, 
            command.ShoppingCartItem.Color, 
            command.ShoppingCartItem.Price, 
            command.ShoppingCartItem.ProductName);

        await repository.SaveChangesAsync(cancellationToken);

        return new AddItemInToBasketResult(shoppingCart.Id);
    }
}
