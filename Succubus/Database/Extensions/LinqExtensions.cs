using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Succubus.Database.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> ConditionalWhere<T>(this IQueryable<T> source, Func<bool> condition,
            Expression<Func<T, bool>> predicate)
        {
            if (condition()) return source.Where(predicate);

            return source;
        }

        public static IQueryable<T> ConditionalWhere<T>(this IQueryable<T> source, bool condition,
            Expression<Func<T, bool>> predicate)
        {
            if (condition) return source.Where(predicate);

            return source;
        }

        public static IEnumerable<T> ConditionalWhere<T>(this IEnumerable<T> source, bool condition,
            Func<T, bool> predicate)
        {
            if (condition) return source.Where(predicate);

            return source;
        }
    }
}