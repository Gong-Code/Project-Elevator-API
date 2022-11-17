using System.Reflection;

namespace ElevatorApi.Services;

public interface IPropertyService
{
    public bool ValidPropertiesExists<TSource, TDestination>(string? fields);
    public bool ValidOrderBy<T>(string orderBy);
}

public class PropertyService : IPropertyService
{
    public bool ValidPropertiesExists<TSource, TDestination>(string? fields)
    {
        return TypeHasProperties<TSource>(fields) && TypeHasProperties<TDestination>(fields);
    }

    public bool ValidOrderBy<T>(string? orderBy)
    {
        if (orderBy is null)
            return false;

        var orderByArr = orderBy.Split(',');
        if (orderByArr.Length is > 2 or 0)
            return false;

        var trimmed = orderByArr[0].Trim();

        return TypeHasProperties<T>(trimmed);
    }

    private static bool TypeHasProperties<T>(string? fields)
    {
        if (fields is null) return false;
        var fieldArr = fields.Split(',');

        return fieldArr.Select(field => field.Trim()).Select(property => typeof(T).GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)).All(propertyInfo => propertyInfo is not null);
    }
}