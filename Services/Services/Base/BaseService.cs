using Microsoft.Extensions.Configuration;
using Repositories;

namespace Services;

public abstract class BaseService {
    protected IRepositoryFactory RepositoryFactory;
    protected IServiceFactory ServiceFactory;
    protected IConfiguration Configuration;
    // public BaseService(IRepositoryFactory repositoryFactory, IServiceFactory serviceFactory) {
    //     this.RepositoryFactory = repositoryFactory;
    //     this.ServiceFactory = serviceFactory;
    // }
    public BaseService(IRepositoryFactory repositoryFactory, IServiceFactory serviceFactory, IConfiguration configuration) {
        this.RepositoryFactory = repositoryFactory;
        this.ServiceFactory = serviceFactory;
        this.Configuration = configuration;
    }
}