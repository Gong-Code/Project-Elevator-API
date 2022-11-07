using Microsoft.AspNetCore.Identity;

namespace ElevatorApi
{
    public interface IUserService
    {
        public Guid GetCurrentUserId();
        public string GetCurrentUserName();
    }

    public class UserService : IUserService
    {
        public Guid GetCurrentUserId()
        {
            // TODO fix
            return Guid.NewGuid();
        }

        public string GetCurrentUserName()
        {
            // TODO fix
            return "Unknown";
        }
    }
}
