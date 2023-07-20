namespace Services;

public interface IServiceFactory
{
    IUserService CreateUserService();
}