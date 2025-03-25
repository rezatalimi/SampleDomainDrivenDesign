using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Configuration;

namespace Sample.API
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected IDistributor Distributor { get; }

        protected BaseController(IDistributor distributor)
        {
            Distributor = distributor;
        }
    }
}
