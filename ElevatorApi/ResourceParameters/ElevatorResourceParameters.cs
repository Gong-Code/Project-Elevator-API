using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.ResourceParameters
{
    public class ElevatorResourceParameters : ResourceParameterBase
    {

        [RegularExpression("enabled|disabled|error", ErrorMessage = "Filter must be enabled, disabled, error.")]
        public string? Filter { get; set; }
        public string OrderBy { get; set; } = "CreatedDateUtc,asc";
        public string? SearchQuery { get; set; }

    }
}
