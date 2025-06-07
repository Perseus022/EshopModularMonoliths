namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto Product)
    : ICommand<CreateProductResult>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{ 
    public CreateProductCommandValidator()
    {
        RuleFor(command => command.Product)
            .NotNull()
            .WithMessage("Product details must be provided.");
        RuleFor(command => command.Product.Name)
            .NotEmpty()
            .WithMessage("Product name is required.");
        RuleFor(command => command.Product.Category)
            .NotEmpty()
            .WithMessage("Product category is required.");
        RuleFor(command => command.Product.ImageFile)
            .NotEmpty()
            .WithMessage("Product image file is required.");
        RuleFor(command => command.Product.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero.");
    }
}

public record CreateProductResult
    (Guid Id);
internal class CreateProductHandler
    (CatalogDbContext dbContext)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {

        // Create a product Entity from Command Object
        // Here you would typically interact with a database or a repository to save the product.
        // Return result with the created product ID.
        var product = CreateNewProduct(command.Product);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }

    private Product CreateNewProduct(ProductDto productDto)
    {
        var product = Product.Create(
            id: Guid.NewGuid(),
            name: productDto.Name,
            category: productDto.Category,
            description: productDto.Description,
            imageFile: productDto.ImageFile,
            price: productDto.Price
        );
        return product;
    }
}
