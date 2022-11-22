using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AoC.Year2021.Day01
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private static int SolvePuzzle(IReadOnlyList<int> input, int offset = 1)
        {
            var counter = 0;
            for (var i = 0; i < input.Count - offset; i++)
                if (input[i] < input[i + offset])
                    counter++;

            return counter;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle(input);
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle(input);
            Assert.AreEqual(1581, result);
        }

        #endregion

        #region Puzzle 2

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle(input, 3);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle(input, 3);
            Assert.AreEqual(1618, result);
        }

        #endregion
    }
}
