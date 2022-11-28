using ElevatorApi.Helpers;
using ElevatorApi.Models.DeviceDtos;
using ElevatorApi.Models.ElevatorDtos;
using ElevatorApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;

namespace ElevatorApi.Controllers;

[Route("api/devices")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly RegistryManager _registryManager;
    private readonly IElevatorRepository _repository;

    public DevicesController(IConfiguration configuration, IElevatorRepository repository, IMapper mapper)
    {
        _configuration = configuration;
        _repository = repository;
        _mapper = mapper;
        _registryManager = RegistryManager.CreateFromConnectionString(configuration["IotHub"]);
    }


    [HttpPost("connect")]
    public async Task<IActionResult> ConnectDevice(DeviceRequest request)
    {
        try
        {
            var elevator = await GetElevatorCreateIfNotExists(request)
                           ?? throw new Exception();

            var device = await GetDeviceCreateIfNotExists(elevator.Id.ToString())
                         ?? throw new Exception();

            var response = new DeviceResponse(device, elevator.Location, _configuration["IotHub"]);

            return Ok(new HttpResponse<DeviceResponse>(response));
        }
        catch
        {
            return StatusCode(500);
        }
    }


    private async Task<Elevator?> GetElevatorCreateIfNotExists(DeviceRequest request)
    {
        Guid.TryParse(request.Id, out var id);

        var elevator = await _repository.GetById(id);

        elevator ??= await _repository.CreateElevator(_mapper.Map<CreateElevatorRequest>(request));

        return elevator;
    }

    private async Task<Device?> GetDeviceCreateIfNotExists(string id)
    {
        return await _registryManager.GetDeviceAsync(id)
               ?? await _registryManager.AddDeviceAsync(new Device(id));
    }
}