using System;
using System.Collections.Generic;

namespace AoC.Util
{
    public static class ArrayHelper
    {
        public static IEnumerable<(int, int)> GetAllPoints<T>(this T[,] array)
        {
            for (var x = 0; x < array.GetLength(0); x++)
            {
                for (var y = 0; y < array.GetLength(1); y++)
                {
                    yield return (x, y);
                }
            }
        }

        public static IEnumerable<(int, int)> GetAdjacentPoints<T>(this T[,] input, int x, int y, bool includeDiagonal)
        {
            foreach (var (dx, dy) in new[] { (0, -1), (-1, 0), (0, 1), (1, 0)})
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

        public static void Print(this int[,] array)
        {
            Console.WriteLine("".PadLeft(array.GetLength(0) * 4, '='));
            for (var x = 0; x < array.GetLength(0); x++)
            {
                var line = "";
                for (var y = 0; y < array.GetLength(1); y++)
                {
                    line += $"[{array[y, x],2}]";
                }

                Console.WriteLine(line);
            }
        }
    }
}
