namespace ElevatorApi.Data.Entities
{
    public static class Enums
    {
        public enum ErrandStatus
        {
            New,
            InProgress,
            Completed
        }

        public enum ElevatorStatus
        {
            Enabled,
            Disabled,
            Error
        }
    }
}
