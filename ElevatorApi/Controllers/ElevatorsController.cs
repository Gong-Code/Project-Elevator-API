using ElevatorApi.Data.Entities;
using ElevatorApi.Models;
using ElevatorApi.Models.Elevator;
using ElevatorApi.Models.Errands;
using ElevatorApi.ResourceParameters;
using ElevatorApi.Services;
using ElevatorApi.Services.Repositories;
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
    public async Task<IActionResult> GetElevators([FromQuery] ElevatorResourceParameters parameters)
    {
        try
        {
            if (!_propertyService.ValidOrderBy<ElevatorDto, ElevatorEntity>(parameters.OrderBy))
                return BadRequest("invalid orderBy does not align with properties on elevator");


            var (elevators, paginationMetadata, isSuccess) = await _elevatorRepository.GetAll(parameters);

            if (!isSuccess)
                throw new Exception();

            return Ok(new PaginatedHttpResponse<IEnumerable<ElevatorDto>>(elevators, paginationMetadata));
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
                if (!_propertyService.ValidOrderBy<ErrandDto, ErrandEntity>(parameters.OrderBy))
                    return BadRequest("invalid orderBy, does not align with properties on errand");

                var (elevator, paginationMetadata) = await _elevatorRepository.GetById(elevatorId, parameters);
                if (elevator is null)
                    return NotFound();

                return Ok(new PaginatedHttpResponse<ElevatorWithErrandsDto>(elevator, paginationMetadata));
            }

            var singleElevator = await _elevatorRepository.GetById(elevatorId);
            if (singleElevator is null)
                return NotFound();

            return Ok(new HttpResponse<ElevatorDto>(singleElevator));
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

