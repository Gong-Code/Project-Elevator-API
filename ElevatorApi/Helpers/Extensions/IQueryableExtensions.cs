
// ReSharper disable InconsistentNaming

using System.Reflection;
using ElevatorApi.ResourceParameters;
using System.Linq.Dynamic.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ElevatorApi.Helpers.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, ResourceParameterBase parameters)
        {
           return query.Skip(parameters.PageSize * (parameters.CurrentPage - 1)).Take(parameters.PageSize);
        }

        public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> query, string orderBy)
        {
            var splitArr = orderBy.Split(',');

            try
            {
               var propertyName = string.Empty;
               var orderAscending = true;
               if (splitArr.Length > 0)
                   propertyName = splitArr[0].Trim().ToLower();
               if (splitArr.Length > 1)
                   orderAscending = splitArr[1].Trim().ToLower() != "desc";


               var propertyInfo = typeof(T).GetProperty(propertyName,
                   BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

               if (propertyInfo is null)
                   throw new Exception();

               return query.OrderBy(propertyName + (orderAscending ? " ascending" : " descending"));

            }
            catch
            {
                // ignored
            }

            return query;
        }
    }
}
