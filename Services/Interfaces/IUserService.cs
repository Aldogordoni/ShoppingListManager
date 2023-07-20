using Business;

namespace Services;

public interface IUserService {
    string Authenticate(AuthenticateRequest request);
}