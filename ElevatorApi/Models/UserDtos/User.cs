namespace ElevatorApi.Models.UserDtos
{
    public class User
    {
        public string Id { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
