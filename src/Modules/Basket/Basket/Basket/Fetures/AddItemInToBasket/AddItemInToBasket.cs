
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

internal class AddItemInToBasket(BasketDbContext dbContext)
    : ICommandHandler<AddItemInToBasketCommand, AddItemInToBasketResult>
{
    public async Task<AddItemInToBasketResult> Handle(AddItemInToBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await dbContext.ShoppingCarts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserName == command.userName, cancellationToken);
        if (shoppingCart == null)
        {
            throw new BasketNotFoundException(command.userName);
        }

        shoppingCart.AddItem(command.ShoppingCartItem.ProductId, 
            command.ShoppingCartItem.Quantity, 
            command.ShoppingCartItem.Color, 
            command.ShoppingCartItem.Price, 
            command.ShoppingCartItem.ProductName);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddItemInToBasketResult(shoppingCart.Id);
    }
}
