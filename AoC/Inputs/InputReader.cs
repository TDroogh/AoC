using System.IO;
using System.Linq;

namespace AoC.Inputs
{
    public static class InputReader
    {
        public static string[] ReadInput(int year, int day, int? puzzle)
        {
            var fileName = $"..\\..\\..\\Year{year}\\Input\\Day{day}";
            if (puzzle.HasValue)
                fileName += $"-{puzzle}";
            fileName += ".txt";

            using var fs = new StreamReader(fileName);
            return fs.ReadToEnd().Split("\r\n");
        }

        public static char[,] ReadArrayInput(int year, int day, int? puzzle, int repeat = 1)
        {
            var lines = ReadInput(year, day, puzzle);
            var characters = lines[0].Length;

            var result = new char[characters * repeat, lines.Length];
            for (var l = 0; l < lines.Length; l++)
                for (var r = 0; r < repeat; r++)
                    for (var c = 0; c < characters; c++)
                        result[c + r * characters, l] = lines[l][c];

            return result;
        }

        public static int[] ReadIntInput(int year, int day, int? puzzle)
        {
            return ReadInput(year, day, puzzle).Select(int.Parse).ToArray();
        }

        public static long[] ReadLongInput(int year, int day, int? puzzle)
        {
            return ReadInput(year, day, puzzle).Select(long.Parse).ToArray();
        }
    }
}
