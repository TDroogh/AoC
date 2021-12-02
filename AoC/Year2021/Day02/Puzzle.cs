using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day02
{
    [TestClass]
    public class Puzzle
    {
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
            Assert.AreEqual(150, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(1250395, result);
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
            Assert.AreEqual(900, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(1451210346, result);
        }

        #endregion
    }
}