using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AoC.Util
{
    public static class InputReader
    {
        public static string[] ReadInput(string suffix = null, [CallerMemberName] string member = null, [CallerFilePath] string filePath = null)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            var directory = Path.GetDirectoryName(filePath)
                ?? throw new ArgumentNullException(nameof(filePath));

            var inputFile = Path.Combine(directory, "input-");
            suffix ??= member.StartsWith("Setup") ? "setup" : "puzzle";
            inputFile += suffix;
            inputFile += ".txt";

            using var fs = new StreamReader(inputFile);
            return fs.ReadToEnd().Split("\r\n");
        }

        public static int[] ReadIntInput(string suffix = null, [CallerMemberName] string member = null, [CallerFilePath] string filePath = null)
        {
            return ReadInput(suffix, member, filePath).Select(int.Parse).ToArray();
        }

        public static long[] ReadLongInput(string suffix = null, [CallerMemberName] string member = null, [CallerFilePath] string filePath = null)
        {
            return ReadInput(suffix, member, filePath).Select(long.Parse).ToArray();
        }

        public static char[,] ReadArrayInput(int repeat = 1, string suffix = null, [CallerMemberName] string member = null, [CallerFilePath] string filePath = null)
        {
            var lines = ReadInput(suffix, member, filePath);
            var characters = lines[0].Length;

            var result = new char[characters * repeat, lines.Length];
            for (var l = 0; l < lines.Length; l++)
                for (var r = 0; r < repeat; r++)
                    for (var c = 0; c < characters; c++)
                        result[c + r * characters, l] = lines[l][c];

            return result;
        }

        public static int[,] ReadIntArrayInput(int repeat = 1, string suffix = null, [CallerMemberName] string member = null, [CallerFilePath] string filePath = null)
        {
            var lines = ReadInput(suffix, member, filePath);
            var characters = lines[0].Length;

            var result = new int[characters * repeat, lines.Length];
            for (var l = 0; l < lines.Length; l++)
                for (var r = 0; r < repeat; r++)
                    for (var c = 0; c < characters; c++)
                        result[c + r * characters, l] = int.Parse(lines[l][c].ToString());

            return result;
        }
    }
}
