namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid ProductId)
    : ICommand<DeleteProductResult>;
public class DeleteProductResult(bool Success)
{
    public bool Success { get; } = Success;
}

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .WithMessage("Product Id must be provided.");
    }
}

internal class DeleteProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteProductCommand,DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        // Retrieve the existing product from the database
        var product = await dbContext.Products.FindAsync(command.ProductId, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(command.ProductId); // Product not found Custom exception Handling
        }
        // Remove the product from the database
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true); // Deletion successful
    }
}
