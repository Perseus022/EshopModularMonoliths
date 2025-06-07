
var builder = WebApplication.CreateBuilder(args);
// Add Serilog for logging
builder.Host.UseSerilog((context, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.

builder.Services.AddCarterWithAssemlies(typeof(CatalogModule).Assembly);
//    typeof(BasketModule).Assembly,
//    typeof(OrderingModule).Assembly
//);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services
    .AddExceptionHandler<CustomExceptionHandler >();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter(); // Map Carter endpoints
app.UseSerilogRequestLogging(); // Add Serilog request logging middleware
app.UseExceptionHandler(options => { }); // Use custom exception handler middleware

app
    .UseCatalogeModule()
    .UseBasketModule()
    .UseOrderingModule();




// Cofigure Error Handling Middleware

app.Run();
