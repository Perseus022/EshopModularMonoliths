var builder = WebApplication.CreateBuilder(args);

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
app.MapCarter();

app
    .UseCatalogeModule()
    .UseBasketModule()
    .UseOrderingModule();

app
    .UseExceptionHandler(options => { });


// Cofigure Error Handling Middleware

app.Run();
