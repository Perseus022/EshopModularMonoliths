
using Keycloak.AuthServices.Authentication;
using Microsoft.Extensions.Configuration;

//using Microsoft.AspNetCore.Authentication.OpenIdConnect;
//using Microsoft.AspNetCore.Authentication;

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
var orderingAssemply = typeof(OrderingModule).Assembly;

builder.Services.AddCarterWithAssemlies(catalogAssemly, basketAssemply, orderingAssemply);

builder.Services.AddMediatRWithAssemblies(catalogAssemly, basketAssemply, orderingAssemply);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services
    .AddMassTransitWithAssemlies(builder.Configuration, catalogAssemly, basketAssemply);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services
    .AddExceptionHandler<CustomExceptionHandler>();

//builder.Services.AddKeycloakWebAppAuthentication(builder.Configuration); Error:  invalid parameter: redirect_uri
builder.Services.AddKeycloakWebApiAuthentication(
    builder.Configuration,
    (options) =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = "myclient";
    }
);
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter(); // Map Carter endpoints
app.UseSerilogRequestLogging(); // Add Serilog request logging middleware
app.UseExceptionHandler(options => { }); // Use custom exception handler middleware
app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization(); // Add authorization middleware

app
    .UseCatalogeModule()
    .UseBasketModule()
    .UseOrderingModule();

// Cofigure Error Handling Middleware

app.Run();
