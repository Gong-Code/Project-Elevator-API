namespace ElevatorApi.Data.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ElevatorEntity : EntityBase
{
    public string Location { get; set; } = null!;
    public Enums.ElevatorStatus ElevatorStatus { get; set; } = Enums.ElevatorStatus.Enabled;
    public IList<ErrandEntity> Errands { get; set; } = new List<ErrandEntity>();
}