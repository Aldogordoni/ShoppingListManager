using Microsoft.AspNetCore.Mvc;
using Services;

namespace Frontend.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IServiceFactory ServiceFactory;
    public BaseController(IServiceFactory serviceFactory)
    {
        this.ServiceFactory = serviceFactory;
    }
}