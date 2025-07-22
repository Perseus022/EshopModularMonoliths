
using Basket.Data.JsonConverters;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Data.Repository
{
    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache)
        : IBasketRepository
    {
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new ShoppingCartConveter() , new ShoppingCartItemConverter() }
        };
        public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            if (!asNoTracking)
            {
                return await repository.GetBasket(userName, false, cancellationToken);
            }
            
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
                // Deserialize the cached basket and return it if it exists
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket,_options)!;

            var basket =  await repository.GetBasket(userName, asNoTracking, cancellationToken);

            // Serialize the basket and store it in the cache
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket,_options), cancellationToken);
            return basket;  
        }

        public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await repository.CreateBasket(basket, cancellationToken);

            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket,_options), cancellationToken);

            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(userName, cancellationToken);

            await cache.RemoveAsync(userName, cancellationToken);

            return true;
        }

        public async Task<int> SaveChangesAsync(string? userName = null,CancellationToken cancellationToken = default)
        {
            var result = await repository.SaveChangesAsync(userName,cancellationToken);

            if(userName != null)
            {
                await cache.RemoveAsync(userName, cancellationToken);    
            }
            return result;
        }
    }
}
