using System.Numerics;

namespace AoC.Util
{
    public static class EnumerableHelper
    {
        public static int Product(this IEnumerable<int> items)
        {
            return items.Aggregate(1, (p, n) => p * n);
        }

        public static int Product<T>(this IEnumerable<T> items, Func<T, int> selector)
        {
            return items.Select(selector).Product();
        }

        public static long Product(this IEnumerable<long> items)
        {
            return items.Aggregate((long)1, (p, n) => p * n);
        }

        public static long Product<T>(this IEnumerable<T> items, Func<T, long> selector)
        {
            return items.Select(selector).Product();
        }

        public static BigInteger Product(this IEnumerable<BigInteger> items)
        {
            return items.Aggregate((BigInteger)1, (p, n) => p * n);
        }

        public static BigInteger Product<T>(this IEnumerable<T> items, Func<T, BigInteger> selector)
        {
            return items.Select(selector).Product();
        }

        public static BigInteger Sum(this IEnumerable<BigInteger> items)
        {
            return items.Aggregate((BigInteger)0, (p, n) => p + n);
        }

        public static BigInteger Sum<T>(this IEnumerable<T> items, Func<T, BigInteger> selector)
        {
            return items.Select(selector).Sum();
        }
    }
}
