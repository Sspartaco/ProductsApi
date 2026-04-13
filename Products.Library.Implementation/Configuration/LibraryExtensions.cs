using Products.Library.Contracts.Services;
using Products.Library.Implementation.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Products.Library.Implementation.Configuration;

public static class LibraryExtensions
{
    public static IServiceCollection AddLibraryServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(LibraryExtensions).Assembly);

        services.AddScoped<IProductService, ProductService>();

        services.AddHttpClient<IGitHubService, GitHubService>(client =>
        {
            client.BaseAddress = new Uri("https://api.github.com/");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("ProductsApi/1.0");
        });

        return services;
    }
}
