using Shared.Exceptions;

namespace Basket.Basket.Exceptions;

public class BasketNotFoundException(string userNamee)
    : NotFoundException("ShoppingCard",userNamee)
{

}
