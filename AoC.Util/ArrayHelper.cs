using System.Text;

#pragma warning disable S2368 // Public methods should not have multidimensional array parameters

namespace AoC.Util
{
    public static class ArrayHelper
    {
        public static IEnumerable<(int x, int y)> GetAllPoints<T>(this T[,] array)
        {
            for (var x = 0; x < array.GetLength(0); x++)
            {
                for (var y = 0; y < array.GetLength(1); y++)
                {
                    yield return (x, y);
                }
            }
        }

        public static IEnumerable<T> GetAllValues<T>(this T[,] array)
        {
            foreach (var (x, y) in array.GetAllPoints())
                yield return array[x, y];
        }

        public static (int x, int y) GetIndex<T>(this T[,] array, T valueToFind) where T : IEquatable<T>
        {
            return array.GetAllPoints().First(c => array[c.Item1, c.Item2].Equals(valueToFind));
        }

        public static IEnumerable<(int x, int y)> FindIndices<T>(this T[,] array, T valueToFind) where T : IEquatable<T>
        {
            return array.GetAllPoints().Where(c => array[c.Item1, c.Item2].Equals(valueToFind));
        }

        public static IEnumerable<(int x, int y)> GetPointsBetween((int x, int y) from, (int x, int y) to)
        {
            if (from.x == to.x)
            {
                var minY = Math.Min(from.y, to.y);
                var maxY = Math.Max(from.y, to.y);

                for (var dy = 0; dy <= maxY - minY; dy++)
                {
                    yield return (from.x, minY + dy);
                }
            }

            if (from.y == to.y)
            {
                var minX = Math.Min(from.x, to.x);
                var maxX = Math.Max(from.x, to.x);

                for (var dx = 0; dx <= maxX - minX; dx++)
                {
                    yield return (minX + dx, from.y);
                }
            }

            if (Math.Abs(from.y - to.y) == Math.Abs(from.x - to.x))
            {
                var diff = Math.Abs(from.y - to.y);
                var posX = from.x < to.x;
                var posY = from.y < to.y;

                for (var d = 0; d <= diff; d++)
                {
                    var x = posX ? from.x + d : from.x - d;
                    var y = posY ? from.y + d : from.y - d;
                    yield return (x, y);
                }
            }
        }

        public static IEnumerable<(int x, int y)> GetPointsAtDistance((int x, int y) from, int distance)
        {
            var left = (from.x - distance, from.y);
            var right = (from.x + distance, from.y);
            var bottom = (from.x, from.y - distance);
            var top = (from.x, from.y + distance);

            return GetPointsBetween(left, bottom)
                .Union(GetPointsBetween(bottom, right))
                .Union(GetPointsBetween(right, top))
                .Union(GetPointsBetween(top, left));
        }

        public static IEnumerable<(int, int)> GetAdjacentPoints<T>(this T[,] input, int x, int y, bool includeDiagonal)
        {
            foreach (var (dx, dy) in new[] { (0, -1), (-1, 0), (0, 1), (1, 0) })
            {
                var x2 = x + dx;
                var y2 = y + dy;

                if (x2 < 0 || x2 >= input.GetLength(0) || y2 < 0 || y2 >= input.GetLength(1))
                    continue;

                yield return (x2, y2);
            }

            if (includeDiagonal)
            {
                foreach (var (dx, dy) in new[] { (1, 1), (-1, 1), (-1, -1), (1, -1) })
                {
                    var x2 = x + dx;
                    var y2 = y + dy;

                    if (x2 < 0 || x2 >= input.GetLength(0) || y2 < 0 || y2 >= input.GetLength(1))
                        continue;

                    yield return (x2, y2);
                }
            }
        }

        public static void Print<T>(this T[,] array, bool withBraces = true, Action<string>? printAction = null)
        {
            printAction ??= Console.WriteLine;

            printAction("".PadLeft(array.GetLength(1), '='));
            for (var x = 0; x < array.GetLength(0); x++)
            {
                var line = new StringBuilder();
                for (var y = 0; y < array.GetLength(1); y++)
                {
                    var toPrint = withBraces ? $"[{array[x, y],2}]" : array[x, y]!.ToString();
                    line.Append(toPrint);
                }

                printAction(line.ToString());
            }
        }
    }
}
