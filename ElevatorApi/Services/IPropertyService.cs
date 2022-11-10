using System.Reflection;

namespace ElevatorApi.Services;

public interface IPropertyService
{
    public bool ValidPropertiesExists<TSource, TDestination>(string? fields);
    public bool ValidOrderBy<TSource, TDestination>(string orderBy);
}

public class PropertyService : IPropertyService
{
    public bool ValidPropertiesExists<TSource, TDestination>(string? fields)
    {
        return TypeHasProperties<TSource>(fields) && TypeHasProperties<TDestination>(fields);
    }

    public bool ValidOrderBy<TSource, TDestination>(string orderBy)
    {
        var orderByArr = orderBy.Split(',');
        if (orderByArr.Length is > 2 or 0)
            return false;

        var trimmed = orderByArr[0].Trim();

        return TypeHasProperties<TSource>(trimmed) && TypeHasProperties<TDestination>(trimmed);
    }

    private static bool TypeHasProperties<T>(string? fields)
    {
        if (fields is null) return false;
        var fieldArr = fields.Split(',');

        foreach (var field in fieldArr)
        {
            var property = field.Trim();

            var propertyInfo = typeof(T).GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public |
                                                               BindingFlags.Instance);

            if (propertyInfo is null)
                return false;
        }

        return true;
    }
}