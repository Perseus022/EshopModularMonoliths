
namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto Product)
    : ICommand<UpdateProductResult>;

public class UpdateProductResult(bool Success)
{
    public bool Success { get; } = Success;
}
internal class UpdateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        // Retrieve the existing product from the database
        var product = await dbContext.Products.FindAsync(command.Product.Id, cancellationToken);
        if (product == null)
        {
            throw new Exception($"Product not found:{command.Product.Id}"); // Product not found
        }
        // Update the product properties
        UpdateProductWithNewValues(product, command.Product);
        // Save changes to the database
        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true); // Update successful
    }

    private void UpdateProductWithNewValues(Product product, ProductDto productDto)
    {
        product.Update(
            productDto.Name,
            productDto.Category,
            productDto.Description,
            productDto.ImageFile,
            productDto.Price);
    }
}
