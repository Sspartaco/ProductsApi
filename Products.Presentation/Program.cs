using System.Globalization;
using Products.Infrastructure.Implementation.Configuration;
using Products.Library.Implementation.Configuration;
using Products.Presentation.Middleware;
using Scalar.AspNetCore;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Products API";
        document.Info.Version = "v1";
        document.Info.Description = "CRUD for Products using Stored Procedures + GitHub API integration.";
        return Task.CompletedTask;
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddLibraryServices();

var app = builder.Build();

app.UseMiddleware<EqExceptionMiddleware>();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Products API");
    options.WithTheme(ScalarTheme.DeepSpace);
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
