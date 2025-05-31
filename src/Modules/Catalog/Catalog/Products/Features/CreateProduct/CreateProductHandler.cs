namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto Product)
    : ICommand<CreateProductResult>;

public record CreateProductResult
    (Guid Id);
internal class CreateProductHandler(CatalogDbContext dbContext)
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
