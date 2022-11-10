using ElevatorApi.Data.Entities;

namespace ElevatorApi.Helpers.Extensions
{
    public static class EnumExtenstions
    {
        public static Enums.ErrandStatus GetErrandStatusAsEnum(this string status)
        {
            return status.ToLower() switch
            {
                "new" => Enums.ErrandStatus.New,
                "inprogress" => Enums.ErrandStatus.InProgress,
                "completed" => Enums.ErrandStatus.Completed,
                _ => Enums.ErrandStatus.New
            };
        }
        public static string GetErrandStatusAsString(this Enums.ErrandStatus status)
        {
            return status switch
            {
                Enums.ErrandStatus.New => "new",
                Enums.ErrandStatus.InProgress => "inprogress",
                Enums.ErrandStatus.Completed => "completed",
                _ => "new"
            };
        }
        public static string GetElevatorStatusAsString(this Enums.ElevatorStatus status)
        {
            return status switch
            {
                Enums.ElevatorStatus.Enabled => "enabled",
                Enums.ElevatorStatus.Disabled => "disabled",
                Enums.ElevatorStatus.Error => "error",
                _ => "enabled"
            };
        }
        public static Enums.ElevatorStatus GetElevatorStatusAsEnum(this string? status)
        {
            return status?.ToLower() switch
            {
                "enabled" => Enums.ElevatorStatus.Enabled,
                "disabled" => Enums.ElevatorStatus.Disabled,
                "error" => Enums.ElevatorStatus.Error,
                _ => Enums.ElevatorStatus.Enabled
            };
        }
    }
}
