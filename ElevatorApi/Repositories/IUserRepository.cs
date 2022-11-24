using ElevatorApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Repositories;

public interface IUserRepository
{
    public Task<(List<UserIdDto>? Users, bool IsSuccess)> GetAllUserIdsAsync(string role);
}

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<(List<UserIdDto>? Users, bool IsSuccess)> GetAllUserIdsAsync(string roleen)
    {
        try
        {
            var roles = _context.Roles.Where(x => x.Name == roleen);

            if (!roles.Any())
                return (null, true);

            var roleId = roles.First().Id;
            var collection = _context.Users
                .Where(u => _context.UserRoles.Any(r => r.RoleId == roleId && r.UserId == u.Id));
            var users = await (from u in collection
                               select new UserIdDto()
                               {
                                   Id = u.Id,
                                   FirstName = _context.UserClaims.First(x => x.UserId == u.Id && x.ClaimType == "given_name")!
                                              .ClaimValue ?? "",
                                   LastName =
                                       _context.UserClaims.First(x => x.UserId == u.Id && x.ClaimType == "family_name")!
                                              .ClaimValue ?? ""
                               }).ToListAsync();
            return (users, true);
        }
        catch
        {
            // ignored
        }
        return (null!, false);
    }
}