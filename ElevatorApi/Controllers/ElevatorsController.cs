using ElevatorApi.Data.Entities;
using ElevatorApi.Helpers;
using ElevatorApi.Models.ElevatorDtos;
using ElevatorApi.Repositories;
using ElevatorApi.ResourceParameters;
using ElevatorApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorApi.Controllers;

[Route("api/elevators")]
[ApiController]
public class ElevatorsController : ControllerBase
{
    private readonly IElevatorRepository _elevatorRepository;
    private readonly IPropertyService _propertyService;

    public ElevatorsController(IElevatorRepository elevatorRepository, IPropertyService propertyService)
    {
        _elevatorRepository = elevatorRepository;
        _propertyService = propertyService;
    }

    [HttpGet]
    [Route("ids")]
    public async Task<IActionResult> GetElevatorIds()
    {
        try
        {
            var (elevators, isSuccess) = await _elevatorRepository.GetAllElevatorIds();

            if (!isSuccess)
                throw new Exception();

            return Ok(new HttpResponse<IEnumerable<ElevatorIds>>(elevators!));
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetElevators([FromQuery] ElevatorResourceParameters parameters)
    {
        try
        {
            if (!_propertyService.ValidOrderBy<ElevatorEntity>(parameters.OrderBy))
                return BadRequest("invalid orderBy does not align with properties on elevator");


            var (elevators, paginationMetadata, isSuccess) = await _elevatorRepository.GetAll(parameters);

            if (!isSuccess)
                throw new Exception();

            return Ok(new PaginatedHttpResponse<IEnumerable<Elevator>>(elevators, paginationMetadata));
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet("{elevatorId:guid}")]
    public async Task<IActionResult> GetById(Guid elevatorId, [FromQuery] ElevatorWithErrandsResourceParameters parameters)
    {
        try
        {
            if (parameters.IncludeErrands)
            {
                if (!_propertyService.ValidOrderBy< ErrandEntity>(parameters.OrderBy))
                    return BadRequest("invalid orderBy, does not align with properties on errand");

                var (elevator, paginationMetadata) = await _elevatorRepository.GetById(elevatorId, parameters);
                if (elevator is null)
                    return NotFound();

                return Ok(new PaginatedHttpResponse<ElevatorWithErrands>(elevator, paginationMetadata));
            }

            var singleElevator = await _elevatorRepository.GetById(elevatorId);
            if (singleElevator is null)
                return NotFound();

            return Ok(new HttpResponse<Elevator>(singleElevator));
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateElevator(CreateElevatorRequest model)
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
    public async Task<IActionResult> UpdateElevator(Guid elevatorId, UpdateElevatorRequest model)
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

