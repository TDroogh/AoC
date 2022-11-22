using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day15
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 40;
            public const int Puzzle1 = 373;
            public const int Setup2 = 315;
            public const int Puzzle2 = 2868;
        }

        #region Puzzle 1

        private object SolvePuzzle1(int[,] input)
        {
            var distances = new int[input.GetLength(0), input.GetLength(1)];
            foreach (var (x, y) in distances.GetAllPoints())
                distances[x, y] = int.MaxValue;
            distances[0, 0] = 0;

            var optimal = distances[input.GetLength(0) - 1, input.GetLength(1) - 1];
            var shouldContinue = true;
            while (shouldContinue)
            {
                foreach (var (x, y) in distances.GetAllPoints())
                {
                    var current = distances[x, y];

                    foreach (var (x2, y2) in distances.GetAdjacentPoints(x, y, false))
                    {
                        var risk = input[x2, y2];
                        var cost = current + risk;

                        if (cost < distances[x2, y2])
                            distances[x2, y2] = cost;
                    }
                }

                var newOptimal = distances[input.GetLength(0) - 1, input.GetLength(1) - 1];
                shouldContinue = optimal > newOptimal;
                optimal = newOptimal;
            }

            return distances[input.GetLength(0) - 1, input.GetLength(1) - 1];
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
            var newInput = new int[input.GetLength(0) * 5, input.GetLength(1) * 5];
            foreach (var (x, y) in input.GetAllPoints())
            {
                var value = input[x, y];
                for (var i = 0; i < 5; i++)
                {
                    for (var j = 0; j < 5; j++)
                    {
                        var x2 = x + i * input.GetLength(0);
                        var y2 = y + j * input.GetLength(1);
                        var v2 = (value - 1 + i + j) % 9 + 1;
                        newInput[x2, y2] = v2;
                    }
                }
            }

            return SolvePuzzle1(newInput);
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