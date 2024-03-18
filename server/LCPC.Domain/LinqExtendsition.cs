using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain
{
    public static class LinqExtendsition
    {
        public static async Task<long> CountAsyncIf<T>(this IEnumerable<T> source, bool isTrue, Func<T, bool> predicate)
        {
            if (isTrue)
            {
                var result = source.LongCount(predicate);
                return await Task.FromResult(result);
            }
            else
            {
                var result = source.LongCount();
                return await Task.FromResult(result);
            }
        }
        public  static  IEnumerable<T>  WhereIf<T>(this IEnumerable<T> source, bool isTrue, Func<T, bool> predicate) {
            if (isTrue)
            {
                var result = source.Where(predicate);
                return result;
            }
           return source;
        }
    }
}