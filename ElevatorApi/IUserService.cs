namespace ElevatorApi;

public interface IUserService
{
    public Guid? GetCurrentUserId();
    public string GetCurrentUserName();
}

public class TempUserService : IUserService
{
    public Guid? GetCurrentUserId()
    {
        return Guid.NewGuid();
    }

    public string GetCurrentUserName()
    {
        return "Unknown";
    }
}

public class IdentityUserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetCurrentUserId()
    {
        var userIdAsString = _httpContextAccessor.HttpContext?.User
            .Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        return Guid.TryParse(userIdAsString, out var result) ? result : null;
    }

    public string GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "Unknown";
    }
}