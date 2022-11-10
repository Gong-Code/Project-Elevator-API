using ElevatorApi.Models.Elevator;
using ElevatorApi.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorApi.Controllers;

[Route("api/elevators")]
[ApiController]
public class ElevatorsController : ControllerBase
{
    private readonly IElevatorRepository _elevatorRepository;

    public ElevatorsController(IElevatorRepository elevatorRepository)
    {
        _elevatorRepository = elevatorRepository;
    }


    [HttpGet]
    public async Task<IActionResult> GetElevators(int pageNumber = 1, int pageSize = 10, string? filterOnStatus = "", string? search = "")
    {
        try
        {
            var (elevators, paginationMetadata, isSuccess) = await _elevatorRepository.GetAll(pageNumber, pageSize, filterOnStatus, search);

            if (!isSuccess)
                throw new Exception();

            return Ok(new { elevators, paginationMetadata });
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet("{elevatorId:guid}")]
    public async Task<IActionResult> GetById(Guid elevatorId, bool includeErrands, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            if (includeErrands)
            {
                var (elevator, paginationMetadata, isSuccess) = await _elevatorRepository.GetById(elevatorId, pageNumber, pageSize);
                if (elevator is null)
                    return NotFound();

                return Ok(new { elevator, paginationMetadata });
            }

            var singleElevator = await _elevatorRepository.GetById(elevatorId);
            if (singleElevator is null)
                return NotFound();

            return Ok(singleElevator);
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateElevator(CreateElevatorDto model)
    {
        try
        {
            var elevator = await _elevatorRepository.CreateElevator(model);
            if (elevator is null)
                throw new Exception();

            return CreatedAtAction(nameof(GetById), new { ElevatorId = elevator.Id }, elevator);
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpPut("{elevatorId:guid}")]
    public async Task<IActionResult> UpdateElevator(Guid elevatorId, UpdateElevatorDto model)
    {
        try
        {
            var result = await _elevatorRepository.UpdateElevator(elevatorId, model);

            if (!result)
                return NotFound();

            return NoContent();
        }
        catch
        {
            return StatusCode(500);
        }
    }
}

