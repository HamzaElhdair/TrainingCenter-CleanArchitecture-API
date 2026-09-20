using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TrainingCenter_API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected string CurrentIp => HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
    }
}
