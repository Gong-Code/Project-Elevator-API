using ElevatorApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Services;

public interface IUserService
{
    public Guid? GetCurrentUserId();
    public Task<string> GetNameForId(string id);
    public Task<(Guid UserId, string UserName)> GetUserData();
    public Task<bool> CheckIfUserExists(string userId);
   
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

        return Guid.TryParse(userIdAsString, out var result) ? result : Guid.Parse("d51bd4e1-af48-4282-8318-dc912d1feae0");
    }

    public async Task<string> GetNameForId(string id)
    {
        var fullName = string.Empty;
        try
        {
            var claims = await _context.UserClaims.Where(x => x.UserId == id).ToListAsync();
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
            fullName = await GetNameForId(userId.ToString());
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

    public Task<Guid> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}