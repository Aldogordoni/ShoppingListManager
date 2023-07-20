using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Repositories;
using Business;

namespace Services;

public class UserService : BaseService, IUserService
{
    public UserService(IRepositoryFactory RepositoryFactory, IServiceFactory ServiceFactory, IConfiguration configuration) : base(RepositoryFactory, ServiceFactory, configuration) { }

    public string Authenticate(AuthenticateRequest request)
    {
        JwtGenerator jwtGenerator = new JwtGenerator(Configuration);

        GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
        settings.Audience = new List<string>() { Configuration["Authentication:Google:ClientId"] };
        GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings).Result;

        return jwtGenerator.CreateUserAuthToken(payload.Email);
    }
}