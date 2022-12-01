using ElevatorApi.Controllers;
using ElevatorApi.Data;
using ElevatorApi.Models.Elevator;
using ElevatorApi.ResourceParameters;

namespace ElevatorApi.Tests.ControllerTests
{
    public class ElevatorsTest
    {
        private readonly SqlDbContext _context;

        private readonly ElevatorsController _sut;

        public ElevatorsTest()
        {
            var elevators = new Mock<ElevatorResourceParameters>();
            
        }
    }
}
