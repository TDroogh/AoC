using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2020.Day11
{
    [TestClass]
    public class Puzzle
    {
        private int CountSeats(char[,] input)
        {
            var totalCount = 0;
            var equal = false;
            var m = 0;

            while (!equal && m++ < 10000)
            {
                var output = CalculateIteration(input);
                equal = true;
                totalCount = 0;
                for (var i = 0; i < input.GetLength(0); i++)
                {
                    for (var j = 0; j < input.GetLength(1); j++)
                    {
                        if (output[i, j] != input[i, j])
                            equal = false;
                        if (output[i, j] == '#')
                            totalCount++;
                    }
                }

                input = output;
            }

            return totalCount;
        }

        private char[,] CalculateIteration(char[,] input)
        {
            var output = new char[input.GetLength(0), input.GetLength(1)];

            for (var i = 0; i < input.GetLength(0); i++)
            {
                for (var j = 0; j < input.GetLength(1); j++)
                {
                    var current = input[i, j];
                    var changed = current;

                    if (current == 'L' && GetAdjacentSeats(input, i, j).All(x => x != '#'))
                    {
                        changed = '#';
                    }

                    if (current == '#' && GetAdjacentSeats(input, i, j).Count(x => x == '#') >= 4)
                    {
                        changed = 'L';
                    }

                    output[i, j] = changed;
                }
            }

            return output;
        }

        private static IEnumerable<char> GetAdjacentSeats(char[,] input, int i, int j)
        {
            var top = i == 0;
            var bottom = i == input.GetLength(0) - 1;
            var left = j == 0;
            var right = j == input.GetLength(1) - 1;

            if (!top)
            {
                if (!left)
                    yield return input[i - 1, j - 1];
                yield return input[i - 1, j];
                if (!right)
                    yield return input[i - 1, j + 1];
            }

            if (!left)
                yield return input[i, j - 1];
            if (!right)
                yield return input[i, j + 1];

            if (!bottom)
            {
                if (!left)
                    yield return input[i + 1, j - 1];
                yield return input[i + 1, j];
                if (!right)
                    yield return input[i + 1, j + 1];
            }
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadArrayInput();
            Assert.AreEqual(37, CountSeats(input));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadArrayInput();
            Assert.AreEqual(2438, CountSeats(input));
        }

        private int CountSeats2(char[,] input)
        {
            var totalCount = 0;
            var equal = false;
            var m = 0;

            while (!equal && m++ < 10000)
            {
                var output = CalculateIteration2(input);
                equal = true;
                totalCount = 0;
                for (var i = 0; i < input.GetLength(0); i++)
                {
                    for (var j = 0; j < input.GetLength(1); j++)
                    {
                        if (output[i, j] != input[i, j])
                            equal = false;
                        if (output[i, j] == '#')
                            totalCount++;
                    }
                }

                input = output;
            }

            return totalCount;
        }

        private char[,] CalculateIteration2(char[,] input)
        {
            var output = new char[input.GetLength(0), input.GetLength(1)];

            for (var i = 0; i < input.GetLength(0); i++)
            {
                for (var j = 0; j < input.GetLength(1); j++)
                {
                    var current = input[i, j];
                    var changed = current;

                    if (current == 'L' && GetAdjacentSeats2(input, i, j).All(x => x != '#'))
                    {
                        changed = '#';
                    }

                    if (current == '#' && GetAdjacentSeats2(input, i, j).Count(x => x == '#') >= 5)
                    {
                        changed = 'L';
                    }

                    output[i, j] = changed;
                }
            }

            return output;
        }

        private static IEnumerable<char> GetAdjacentSeats2(char[,] input, int i, int j)
        {
            yield return GetFirstSeat(input, i, j, -1, -1);
            yield return GetFirstSeat(input, i, j, 0, -1);
            yield return GetFirstSeat(input, i, j, 1, -1);

            yield return GetFirstSeat(input, i, j, -1, 0);
            yield return GetFirstSeat(input, i, j, 1, 0);

            yield return GetFirstSeat(input, i, j, -1, 1);
            yield return GetFirstSeat(input, i, j, 0, 1);
            yield return GetFirstSeat(input, i, j, 1, 1);
        }

        private static char GetFirstSeat(char[,] input, int i, int j, int offsetI, int offsetJ)
        {
            var k = i + offsetI;
            var l = j + offsetJ;
            while (k >= 0 && k < input.GetLength(0) && l >= 0 && l < input.GetLength(1))
            {
                var current = input[k, l];
                if (current != '.')
                    return current;

                k += offsetI;
                l += offsetJ;
            }

            return '.';
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadArrayInput();
            Assert.AreEqual(26, CountSeats2(input));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadArrayInput();
            Assert.AreEqual(2174, CountSeats2(input));
        }
    }
}
