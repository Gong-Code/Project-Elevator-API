using ElevatorApi.Helpers;
using ElevatorApi.Models.UserDtos;
using ElevatorApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        [Route("ids")]
        public async Task<IActionResult> GetAllUser(string role = "admin")
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
    }
}
