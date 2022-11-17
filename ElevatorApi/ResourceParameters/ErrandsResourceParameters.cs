namespace ElevatorApi.ResourceParameters;

public class ErrandsResourceParameters : ResourceParameterBase
{
    public string? Filter { get; set; }
    public string OrderBy { get; set; } = "CreatedDateUtc,asc";
    public string? SearchQuery { get; set; }

}