using Microsoft.Extensions.DependencyInjection;

namespace Services;
public static partial class IServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IServiceFactory, ServiceFactory>();
    }
}