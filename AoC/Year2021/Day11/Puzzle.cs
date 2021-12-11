using System;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day11
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup10 = 204;
            public const int Setup100 = 1656;
            public const int Puzzle1 = 1603;
            public const int Setup2 = 195;
            public const int Puzzle2 = 222;
        }

        #region Puzzle 1

        private object SolvePuzzle1(int[,] input, int iterations)
        {
            var field = new int[input.GetLength(0), input.GetLength(1)];
            foreach (var (x, y) in input.GetAllPoints())
                field[x, y] = input[x, y];
            var flashCount = 0;
            for (var i = 0; i < iterations; i++)
            {
                flashCount = Loop(field, flashCount, i);
            }

            return flashCount;
        }

        private static int Loop(int[,] field, int flashCount, int i)
        {
            foreach (var (x, y) in field.GetAllPoints())
            {
                field[x, y]++;
                if (field[x, y] == 10)
                    Flash(field, x, y, ref flashCount);
            }

            if (i + 1 % 10 == 0)
                field.Print();

            foreach (var (x, y) in field.GetAllPoints())
            {
                if (field[x, y] >= 10)
                    field[x, y] = 0;
            }

            if ((i + 1) % 10 == 0)
                field.Print();
            Console.WriteLine($"After iteration {i + 1}: {flashCount}");
            return flashCount;
        }

        private static void Flash(int[,] field, int x, int y, ref int flashCount)
        {
            flashCount++;
            foreach (var (x2, y2) in field.GetAdjacentPoints(x, y, true))
            {
                field[x2, y2]++;
                if (field[x2, y2] == 10 && flashCount < 1_000_000)
                    Flash(field, x2, y2, ref flashCount);
            }
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadIntArrayInput();
            var result10 = SolvePuzzle1(input, 10);
            Assert.AreEqual(Results.Setup10, result10);
            var result100 = SolvePuzzle1(input, 100);
            Assert.AreEqual(Results.Setup100, result100);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadIntArrayInput();
            var result = SolvePuzzle1(input, 100);
            Assert.AreEqual(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(int[,] input)
        {
            var field = new int[input.GetLength(0), input.GetLength(1)];
            foreach (var (x, y) in input.GetAllPoints())
                field[x, y] = input[x, y];
            var flashCount = 0;
            for (var i = 0; i < 10_000; i++)
            {
                var currentFlashCunt = flashCount;
                flashCount = Loop(field, flashCount, i);
                if (flashCount - currentFlashCunt == 100)
                    return i + 1;
            }

            return -1;
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