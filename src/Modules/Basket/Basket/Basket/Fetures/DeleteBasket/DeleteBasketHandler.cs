
namespace Basket.Basket.Fetures.DeleteBasket;

public record DeleteBasketCommand(string UserName) 
    : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool Success);

public class DeleteBasketHandler(IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {

        await repository.DeleteBasket(command.UserName, cancellationToken);

        return new DeleteBasketResult(true);


    }
}
