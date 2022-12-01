using ElevatorApi.Helpers;
using ElevatorApi.Models.UserDtos;
using ElevatorApi.Repositories;
using ElevatorApi.ResourceParameters;
using ElevatorApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorApi.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPropertyService _propertyService;
    

    public UsersController(IUserRepository userRepository, IPropertyService propertyService)
    {
        _userRepository = userRepository;
        _propertyService = propertyService;
        

    }


    [HttpGet]
    [Route("ids")]
    public async Task<IActionResult> GetAllUserIds(string role = "admin")
    {
        try
        {
            var (users, isSuccess) = await _userRepository.GetAllUserIdsAsync(role);

            if (!isSuccess)
                throw new Exception();

            return Ok(new HttpResponse<IEnumerable<UserIds>>(users ?? new List<UserIds>()));
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserResourceParameters parameters)
    {
        try
        {      
            var (users, paginationMetadata, isSuccess) = await
                _userRepository.GetAllAsync(parameters);

            if (!isSuccess)
                throw new Exception();

            return Ok(new PaginatedHttpResponse<IEnumerable<User>>(users, paginationMetadata));
        }
        catch
        {
            return StatusCode(500);
        }
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateUser([FromBody]CreateUserRequest model )
    //{
    //    try
    //    {

    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(e);
    //        throw;
    //    }
    //}
}


