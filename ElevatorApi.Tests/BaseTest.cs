using ElevatorApi.Services;

namespace ElevatorApi.Tests
{
    public class BaseTest
    {
        protected readonly Fixture Fixture;
        protected readonly Guid UserGuid = Guid.Parse("4a6547f6-26f7-43e2-91a5-175b20d55240");
        protected readonly string UserName = "Test User";
        protected readonly Mock<IUserService> UserService;

        protected BaseTest()
        {
            Fixture = new Fixture();

            UserService = new Mock<IUserService>();
            UserService.Setup(x => x.GetCurrentUserId()).Returns(UserGuid);
            UserService.Setup(x => x.GetNameForId(UserGuid.ToString())).Returns(Task.FromResult(UserName));
            UserService.Setup(x => x.GetUserData()).Returns(Task.FromResult((_userGuid: UserGuid, _userName: UserName)));

        }
    }
}
