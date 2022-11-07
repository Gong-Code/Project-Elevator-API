using ElevatorApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly SqlDbContext _context;

        public CommentsController(SqlDbContext context)
        {
            _context = context;
        }
    }
}
