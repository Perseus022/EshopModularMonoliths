namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid ProductId)
    : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool Success);
internal class DeleteProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteProductCommand,DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        // Retrieve the existing product from the database
        var product = await dbContext.Products.FindAsync(command.ProductId, cancellationToken);
        if (product == null)
        {
            throw new Exception($"Product not found: {command.ProductId}"); // Product not found
        }
        // Remove the product from the database
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true); // Deletion successful
    }
}
