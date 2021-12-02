using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day02
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 150;
            public const int Puzzle1 = 1_250_395;
            public const int Setup2 = 900;
            public const int Puzzle2 = 1_451_210_346;
        }

        #region Puzzle 1

        private long SolvePuzzle1(string[] input)
        {
            var horizontal = 0;
            var vertical = 0;

            foreach (var line in input)
            {
                var split = line.Split(' ');
                switch (split[0])
                {
                    case "forward":
                        horizontal += int.Parse(split[1]);
                        break;
                    case "up":
                        vertical -= int.Parse(split[1]);
                        break;
                    case "down":
                        vertical += int.Parse(split[1]);
                        break;
                }
            }

            return horizontal * vertical;
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
            var horizontal = 0;
            var vertical = 0;
            var aim = 0;

            foreach (var line in input)
            {
                var split = line.Split(' ');
                switch (split[0])
                {
                    case "forward":
                        horizontal += int.Parse(split[1]);
                        vertical += aim * int.Parse(split[1]);
                        break;
                    case "up":
                        aim -= int.Parse(split[1]);
                        break;
                    case "down":
                        aim += int.Parse(split[1]);
                        break;
                }
            }

            return horizontal * vertical;
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