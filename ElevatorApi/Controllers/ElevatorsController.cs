using ElevatorApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ElevatorsController : ControllerBase
{
    private readonly SqlDbContext _context;

    public ElevatorsController(SqlDbContext context)
    {
        _context = context;
    }
}

