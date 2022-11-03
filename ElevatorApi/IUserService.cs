namespace ElevatorApi
{
    public interface IUserService
    {
        public Guid GetCurrentUser();
    }

    public class UserService : IUserService
    {
        public Guid GetCurrentUser()
        {
            return Guid.NewGuid();
        }
    }
}
