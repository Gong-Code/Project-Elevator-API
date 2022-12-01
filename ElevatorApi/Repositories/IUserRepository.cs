using System.Linq.Expressions;
using ElevatorApi.Data;
using ElevatorApi.Data.Entities;
using ElevatorApi.Helpers;
using ElevatorApi.Helpers.Extensions;
using ElevatorApi.Models.UserDtos;
using ElevatorApi.ResourceParameters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Repositories;

public interface IUserRepository
{
    public Task<(List<UserIds>? Users, bool IsSuccess)> GetAllUserIdsAsync(string role);
    public Task<(IEnumerable<User> Users, PaginationMetadata paginationMetadata, bool isSuccess)> GetAllAsync(UserResourceParameters parameters);
    public Task GetAsync(Expression<Func<IdentityUser, bool>> filter);
}

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<User> Users, PaginationMetadata paginationMetadata, bool isSuccess)> GetAllAsync(UserResourceParameters parameters)
    {
        try
        {
            var users = await _context.Users.Select(u => new User
            {
                Id = u.Id,
                FirstName = _context.UserClaims.First(x => x.UserId == u.Id && x.ClaimType == "given_name")!
                                              .ClaimValue ?? "",
                LastName = _context.UserClaims.First(x => x.UserId == u.Id && x.ClaimType == "family_name")!
                                              .ClaimValue ?? "",
                Role = _context.Roles.FirstOrDefault(x => x.Id == _context.UserRoles.FirstOrDefault(x => x.UserId == u.Id).RoleId).Name
            }).OrderBy(x => x.FirstName).ApplyPagination(parameters).ToListAsync();

            var paginationMetadata = new PaginationMetadata(parameters, await _context.Users.CountAsync());


            return (users, paginationMetadata,true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<(List<UserIds>? Users, bool IsSuccess)> GetAllUserIdsAsync(string role)
    {
        try
        {
            var roles = _context.Roles.Where(x => x.Name == role);

            if (!roles.Any())
                return (null, true);

            var roleId = roles.First().Id;
            var collection = _context.Users
                .Where(u => _context.UserRoles.Any(r => r.RoleId == roleId && r.UserId == u.Id));
            var users = await (from u in collection
                               select new UserIds()
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

    public Task GetAsync(Expression<Func<IdentityUser, bool>> filter)
    {
        throw new NotImplementedException();
    }
}