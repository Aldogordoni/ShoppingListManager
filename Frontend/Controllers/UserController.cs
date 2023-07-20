using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Business;
using Services;

namespace Frontend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    public UserController(IServiceFactory ServiceFactory) : base(ServiceFactory) { }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody] AuthenticateRequest data)
    {
        string authToken = ServiceFactory.CreateUserService().Authenticate(data);
        return Ok(new { AuthToken = authToken });
    }

    [Authorize]
    [HttpGet("test-authentication")]
    public IActionResult TestAuthentication()
    {
        return Ok(new { Success = true });
    }
}
