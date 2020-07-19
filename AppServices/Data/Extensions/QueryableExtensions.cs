using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppServices.Data.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Searches in all string properties for the specifed search key.
        /// It is also able to search for several words. If the searchKey is for example 'John Travolta' then
        /// all records which contain either 'John' or 'Travolta' in some string property
        /// are returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="searchKey">The keyword to be search</param>
        /// <returns></returns>
        public static IQueryable<T> FullTextSearch<T>(this IQueryable<T> queryable, string searchKey)
        {
            return FullTextSearch(queryable, searchKey, false);
        }


        /// <summary>
        /// Searches in all string properties for the specifed search key.
        /// It is also able to search for several words. If the searchKey is for example 'John Travolta' then
        /// with exactMatch set to false all records which contain either 'John' or 'Travolta' in some string property
        /// are returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="searchKey">The keyword to be search</param>
        /// <param name="exactMatch">Specifies if only the whole word or every single word should be searched.</param>
        /// <returns></returns>
        public static IQueryable<T> FullTextSearch<T>(this IQueryable<T> queryable, string searchKey, bool exactMatch)
        {
            if (string.IsNullOrWhiteSpace(searchKey)) return queryable;

            var parameter = Expression.Parameter(typeof(T), "c");

            var indexOfMethod = typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) });


            var publicProperties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(p => p.PropertyType == typeof(string));

            var searchKeyParts = exactMatch ? new[] { searchKey } : searchKey.Split(' ');

            var orExpressions = (from property in publicProperties
                                 select Expression.Property(parameter, property)
                into nameProperty
                                 from searchKeyPart in searchKeyParts
                                 let searchKeyExpression = Expression.Constant(searchKeyPart)
                                 let ignoreCaseExpression = Expression.Constant(StringComparison.CurrentCultureIgnoreCase)
                                 let callIndexOfExpression = Expression.Call(nameProperty, indexOfMethod, searchKeyExpression, ignoreCaseExpression)
                                 select Expression.GreaterThanOrEqual(callIndexOfExpression, Expression.Constant(0)))
                    .Aggregate<Expression, Expression>(null,
                        (current, callContainsMethod) =>
                        current == null ? callContainsMethod : Expression.Or(current, callContainsMethod));

            var whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new[] { queryable.ElementType },
                queryable.Expression,
                Expression.Lambda<Func<T, bool>>(orExpressions, parameter));

            return queryable.Provider.CreateQuery<T>(whereCallExpression);
        }

        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), string.Empty);

            #region Sort by sub properties
            //This part is what sorts by sub properties
            //For example: Manufacturer.Name
            var parts = propertyName.Split('.');

            Expression parent = param;

            foreach (var part in parts)
            {
                parent = Expression.Property(parent, part);
            }

            LambdaExpression sort = Expression.Lambda(parent, param);
            #endregion

            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new[] { typeof(T), parent.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, false);
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, false);
        }
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, true);
        }
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, true);
        }
    }
}
