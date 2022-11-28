using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models.DeviceDtos;

public class DeviceRequest
{
    [StringLength(200, MinimumLength = 2)]
    public string? Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Location { get; set; } = null!;

    [Required]
    [Range(2, 20)]
    public int NumberOfFloors { get; init; }
}