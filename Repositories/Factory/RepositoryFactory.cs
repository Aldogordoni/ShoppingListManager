using Microsoft.Extensions.Configuration;

namespace Repositories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly IConfiguration configuration;
    public RepositoryFactory(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
}