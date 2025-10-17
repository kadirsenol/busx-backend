using System.Linq.Expressions;
namespace BusX.AppService.Extensions
{
    public static class ExtIQueryable
    {
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate) => condition ? source.Where(predicate) : source;
    }
}