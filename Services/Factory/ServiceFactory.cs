using Microsoft.Extensions.Configuration;
using Repositories;

namespace Services;

public class ServiceFactory : IServiceFactory
{
    private readonly IConfiguration configuration;
    private readonly IRepositoryFactory repositoryFactory;
    public ServiceFactory(IConfiguration configuration, IRepositoryFactory repositoryFactory)
    {
        this.configuration = configuration;
        this.repositoryFactory = repositoryFactory;
    }

    public IUserService CreateUserService()
    {
        return new UserService(repositoryFactory, this, configuration);
    }
}