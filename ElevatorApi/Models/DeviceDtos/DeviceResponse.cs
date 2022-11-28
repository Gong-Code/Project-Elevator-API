// ReSharper disable MemberCanBePrivate.Global
namespace ElevatorApi.Models.DeviceDtos;

public class DeviceResponse
{
    public DeviceResponse(Microsoft.Azure.Devices.Device device, string location, string iotHub)
    {
        Location = location;
        Id = device.Id;
        ConnectionString = $"HostName={iotHub?.Split(";")[0].Split("=")[1]};DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";
    }
    public string Id { get; set; }
    public string Location { get; set; }
    public string ConnectionString { get; set; }
}