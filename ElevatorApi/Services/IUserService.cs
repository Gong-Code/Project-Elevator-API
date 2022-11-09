using ElevatorApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Services;

public interface IUserService
{
    public Guid? GetCurrentUserId();
    public Task<string> GetCurrentUserName();
    public Task<(Guid UserId, string UserName)> GetUserData();
    public Task<bool> CheckIfUserExists(string userId);
}

public class TempUserService : IUserService
{
    public Guid? GetCurrentUserId()
    {
        return Guid.NewGuid();
    }

    public Task<string> GetCurrentUserName()
    {
        return Task.FromResult("Unknown");
    }

    public Task<(Guid UserId, string UserName)> GetUserData()
    {
        throw new NotImplementedException();
    }

    public Task<bool> CheckIfUserExists(string userId)
    {
        throw new NotImplementedException();
    }
}

public class IdentityUserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserDbContext _context;


    public IdentityUserService(IHttpContextAccessor httpContextAccessor, UserDbContext context)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetCurrentUserId()
    {
        var userIdAsString = _httpContextAccessor.HttpContext?.User
            .Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        return Guid.TryParse(userIdAsString, out var result) ? result : null;
    }

    public async Task<string> GetCurrentUserName()
    {
        var fullName = string.Empty;
        try
        {
            var userId = GetCurrentUserId() ?? Guid.Empty;

            var claims = await _context.UserClaims.Where(x => x.UserId == userId.ToString()).ToListAsync();
            var name = claims.FirstOrDefault(x => x.ClaimType == "given_name")?.ClaimValue ?? "";
            var lastName = claims.FirstOrDefault(x => x.ClaimType == "family_name")?.ClaimValue ?? "";
            fullName = name + " " + lastName;
        }
        catch
        {
            // ignored
        }
        return fullName;
    }

    public async Task<(Guid UserId, string UserName)> GetUserData()
    {
        var userId = GetCurrentUserId() ?? Guid.Empty;
        var fullName = string.Empty;

        try
        {
            fullName = await GetCurrentUserName();
        }
        catch
        {
            // ignored
        }

        return (userId, fullName);
    }

    public async Task<bool> CheckIfUserExists(string userId)
    {
        var exists = false;
        try
        {
            var user = await _context.Users.FindAsync(userId);

            if (user is not null)
                exists = true;
        }
        catch
        {
            // Ignored
        }

        return exists;
    }
}