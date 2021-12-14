using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day14
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const long Setup1 = 1588;
            public const long Puzzle1 = 2375;
            public const long Setup2 = 2_188_189_693_529;
            public const long Puzzle2 = 2;
        }

        #region Puzzle 1

        private object SolvePuzzle(string[] input, int iterations)
        {
            var template = input[0];
            var rules = new Dictionary<(char, char), char>();
            foreach (var line in input.Skip(2))
            {
                var split = line.Split(" -> ");
                rules.Add((split[0][0], split[0][1]), split[1][0]);
            }

            for (var i = 0; i < iterations; i++)
            {
                var sb = new StringBuilder();

                sb.Append(template[0]);
                for (var c = 0; c < template.Length - 1; c++)
                {
                    var rule = rules[(template[c], template[c + 1])];
                    sb.Append(rule);
                    sb.Append(template[c + 1]);
                }

                template = sb.ToString();
            }

            var dict = template.GroupBy(x => x).ToDictionary(x => x.Key, y => y.LongCount());
            var max = dict.Max(x => x.Value);
            var min = dict.Min(x => x.Value);
            return max - min;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle(input, 10);
            Assert.AreEqual(Results.Setup1, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle(input, 10);
            Assert.AreEqual(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle(input, 20);
            Assert.AreEqual(Results.Setup2, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle(input, 10);
            Assert.AreEqual(Results.Puzzle2, result);
        }

        #endregion
    }
}