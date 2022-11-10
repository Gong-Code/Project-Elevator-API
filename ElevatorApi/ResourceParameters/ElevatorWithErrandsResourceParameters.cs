using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.ResourceParameters
{
    public class ElevatorWithErrandsResourceParameters : ResourceParameterBase
    {
        public bool IncludeErrands { get; set; }
        [RegularExpression("new|inprogress|completed", ErrorMessage = "Filter must be new, inprogress, completed.")]
        public string? Filter { get; set; }
        public string OrderBy { get; set; } = "CreatedDateUtc,asc";
    }
}
