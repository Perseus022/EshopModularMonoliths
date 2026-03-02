
var builder = WebApplication.CreateBuilder(args);
// Add Serilog for logging
builder.Host.UseSerilog((context, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.

// Common Services Carter, FluentValidation, MediatR, Serilog
var catalogAssemly = typeof(CatalogModule).Assembly;
var basketAssemply = typeof(BasketModule).Assembly;

builder.Services.AddCarterWithAssemlies(catalogAssemly, basketAssemply);

builder.Services.AddMediatRWithAssemblies(catalogAssemly, basketAssemply);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services
    .AddMassTransitWithAssemlies(catalogAssemly,basketAssemply);

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
