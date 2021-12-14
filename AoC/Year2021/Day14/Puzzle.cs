using System.Collections.Generic;
using System.Linq;
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
            public const long Puzzle2 = 1_976_896_901_756;
        }

        #region Puzzle 1

        private object SolvePuzzle(string[] input, int iterations)
        {
            var templateText = input[0];
            var rules = new Dictionary<(char, char), char>();
            foreach (var line in input.Skip(2))
            {
                var split = line.Split(" -> ");
                rules.Add((split[0][0], split[0][1]), split[1][0]);
            }
            
            //Instead of evaluating all values 1 by 1, we evaluate the total count of each pair
            var templateCounts = new Dictionary<(char, char), long>();
            for (var c = 0; c < templateText.Length - 1; c++)
            {
                var key = (templateText[c], templateText[c + 1]);
                templateCounts.AddOrIncrement(key);
            }

            for (var i = 0; i < iterations; i++)
            {
                var newCounts = new Dictionary<(char, char), long>();
                foreach (var (key, count) in templateCounts)
                {
                    var rule = rules[key];
                    newCounts.AddOrIncrement((key.Item1, rule), count);
                    newCounts.AddOrIncrement((rule, key.Item2), count);
                }

                templateCounts = newCounts;
            }

            var charCounts = new Dictionary<char, long>();
            foreach (var ((key1, key2), count) in templateCounts)
            {
                charCounts.AddOrIncrement(key1, count);
                charCounts.AddOrIncrement(key2, count);
            }

            //All values are duplicated in the counts
            foreach (var (key, value) in charCounts)
                charCounts[key] = value / 2;

            //The first and last characters are not duplicated, so add 1 for them 
            charCounts.AddOrIncrement(templateText.First());
            charCounts.AddOrIncrement(templateText.Last());

            var max = charCounts.Max(x => x.Value);
            var min = charCounts.Min(x => x.Value);

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
            var result = SolvePuzzle(input, 40);
            Assert.AreEqual(Results.Setup2, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle(input, 40);
            Assert.AreEqual(Results.Puzzle2, result);
        }

        #endregion
    }
}