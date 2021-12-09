using System.Collections.Generic;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day09
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 15;
            public const int Puzzle1 = 448;
            public const int Setup2 = 1134;
            public const int Puzzle2 = 1417248;
        }

        #region Puzzle 1

        private object SolvePuzzle1(char[,] input)
        {
            var sum = 0;
            for (var x = 0; x < input.GetLength(0); x++)
            {
                for (var y = 0; y < input.GetLength(1); y++)
                {
                    var isLowest = true;
                    var value = input[x, y];
                    if (x > 0 && value >= input[x - 1, y])
                        isLowest = false;
                    if (y > 0 && value >= input[x, y - 1])
                        isLowest = false;
                    if (x < input.GetLength(0) - 1 && value >= input[x + 1, y])
                        isLowest = false;
                    if (y < input.GetLength(1) - 1 && value >= input[x, y + 1])
                        isLowest = false;

                    if (isLowest)
                        sum += 1 + int.Parse(value.ToString());
                }
            }

            return sum;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Setup1, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(char[,] input)
        {
            var basinSizes = new List<int>();

            for (var x = 0; x < input.GetLength(0); x++)
            {
                for (var y = 0; y < input.GetLength(1); y++)
                {
                    var isLowest = true;
                    var value = input[x, y];
                    if (x > 0 && value >= input[x - 1, y])
                        isLowest = false;
                    if (y > 0 && value >= input[x, y - 1])
                        isLowest = false;
                    if (x < input.GetLength(0) - 1 && value >= input[x + 1, y])
                        isLowest = false;
                    if (y < input.GetLength(1) - 1 && value >= input[x, y + 1])
                        isLowest = false;

                    if (isLowest)
                    {
                        var points = new List<(int, int)>();
                        FindBasin(input, x, y, points);

                        basinSizes.Add(points.Distinct().Count());
                    }
                }
            }
            
            basinSizes = basinSizes.OrderByDescending(i => i).Take(3).ToList();
            return basinSizes[0] * basinSizes[1] * basinSizes[2];
        }

        private void FindBasin(char[,] input, int x, int y, List<(int, int)> points, int iteration = 0)
        {
            if (iteration == 100)
                return;

            points.Add((x, y));
            var value = input[x, y];
            iteration++;

            foreach (var diff in new[] { (0, -1), (-1, 0), (0, 1), (1, 0) })
            {
                var x2 = x + diff.Item1;
                var y2 = y + diff.Item2;

                if (x2 < 0 || x2 >= input.GetLength(0) || y2 < 0 || y2 >= input.GetLength(1))
                    continue;

                var value2 = input[x2, y2];
                if (value < value2 && value2 != '9')
                    FindBasin(input, x2, y2, points, iteration);
            }
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(Results.Setup2, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(Results.Puzzle2, result);
        }

        #endregion
    }
}