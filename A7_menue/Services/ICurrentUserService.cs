using System.Security.Claims;

namespace A7_menue.Services;

public interface ICurrentUserService
{
    int? TenantId { get; }
    int? UserId { get; }
}

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public int? TenantId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User?.FindFirst("TenantId")?.Value;
            return value != null ? int.Parse(value) : null;
        }
    }

    public int? UserId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return value != null ? int.Parse(value) : null;
        }
    }
}

