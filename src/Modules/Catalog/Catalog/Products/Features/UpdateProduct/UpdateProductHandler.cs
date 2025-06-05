namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto Product)
    : ICommand<UpdateProductResult>;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id)
            .NotEmpty()
            .WithMessage("Product Id must be provided.");
        RuleFor(x => x.Product.Name)
            .NotEmpty()
            .WithMessage("Product name is required.");
        RuleFor(x => x.Product.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero.");
    }
}


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
            throw new ProductNotFoundException(command.Product.Id); // Product not found Custom exception Handling
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
