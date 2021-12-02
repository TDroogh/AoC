using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2019.Day
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private object SolvePuzzle1(int[] input)
        {
            var total = 0;

            foreach (var mass in input)
            {
                var fuel = CalculateFuel(mass);
                total += fuel;
            }

            return total;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(2 + 2 + 654 + 33583, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(3361299, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(int[] input)
        {
            var total = 0;
            foreach (var mass in input)
            {
                var fuel = CalculateFuel(mass);
                while (fuel > 0)
                {
                    total += fuel;
                    fuel = CalculateFuel(fuel);
                }
            }

            return total;
        }

        private static int CalculateFuel(int mass)
        {
            return (int)System.Math.Floor(mass / 3m) - 2;
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(2 + 2 + 966 + 50346, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(5039071, result);
        }

        #endregion
    }
}