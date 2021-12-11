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

        private object SolvePuzzle1(int[,] input)
        {
            var sum = 0;
            for (var x = 0; x < input.GetLength(0); x++)
            {
                for (var y = 0; y < input.GetLength(1); y++)
                {
                    var value = input[x, y];
                    var isLowest = GetAdjacentPoints(input, x, y).All(point => value < input[point.Item1, point.Item2]);

                    if (isLowest)
                        sum += 1 + value;
                }
            }

            return sum;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadIntArrayInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Setup1, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadIntArrayInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(int[,] input)
        {
            var basinSizes = new List<int>();

            for (var x = 0; x < input.GetLength(0); x++)
            {
                for (var y = 0; y < input.GetLength(1); y++)
                {
                    var value = input[x, y];
                    var isLowest = GetAdjacentPoints(input, x, y).All(point => value < input[point.Item1, point.Item2]);

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

        private void FindBasin(int[,] input, int x, int y, List<(int, int)> points, int iteration = 0)
        {
            if (iteration == 100)
                return;

            points.Add((x, y));
            var value = input[x, y];
            iteration++;

            foreach (var (x2, y2) in GetAdjacentPoints(input, x, y))
            {
                var value2 = input[x2, y2];

                if (value < value2 && value2 != '9')
                    FindBasin(input, x2, y2, points, iteration);
            }
        }

        private IEnumerable<(int, int)> GetAdjacentPoints(int[,] input, int x, int y)
        {
            foreach (var (dx, dy) in new[] { (0, -1), (-1, 0), (0, 1), (1, 0) })
            {
                var x2 = x + dx;
                var y2 = y + dy;

                if (x2 < 0 || x2 >= input.GetLength(0) || y2 < 0 || y2 >= input.GetLength(1))
                    continue;

                yield return (x2, y2);
            }
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadIntArrayInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(Results.Setup2, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadIntArrayInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(Results.Puzzle2, result);
        }

        #endregion
    }
}