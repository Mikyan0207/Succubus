using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Succubus.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> ConditionalWhere<T>(this IQueryable<T> source, Func<bool> condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition() ? source.Where(predicate) : source;
        }

        public static IQueryable<T> ConditionalWhere<T>(this IQueryable<T> source, bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IEnumerable<T> ConditionalWhere<T>(this IEnumerable<T> source, bool condition,
            Func<T, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IAsyncEnumerable<T> ConditionalWhere<T>(this IAsyncEnumerable<T> source, bool condition,
            Func<T, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}