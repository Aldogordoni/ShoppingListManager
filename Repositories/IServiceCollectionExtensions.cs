using Microsoft.Extensions.DependencyInjection;

namespace Repositories;
public static partial class IServiceCollectionExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IRepositoryFactory, RepositoryFactory>();
    }
}
