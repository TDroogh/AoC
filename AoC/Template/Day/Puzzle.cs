using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Template.Day
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 1;
            public const int Puzzle1 = 1;
            public const int Setup2 = 2;
            public const int Puzzle2 = 2;
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            return 1;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Setup1, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input)
        {
            return 2;
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(Results.Setup2, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(Results.Puzzle2, result);
        }

        #endregion
    }
}