using System.Collections.Generic;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day06
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const long Setup1 = 5934;
            public const long Puzzle1 = 359999;
            public const long Setup2 = 26_984_457_539;
            public const long Puzzle2 = 1_631_647_919_273;
        }

        private Dictionary<int, long> ParseInitialState(string input)
        {
            var split = input.Split(",");
            var dict = new Dictionary<int, long>();

            foreach (var value in split.Select(int.Parse))
            {
                if (dict.ContainsKey(value))
                    dict[value] += 1;
                else
                    dict[value] = 1;
            }

            return dict;
        }

        private Dictionary<int, long> GetNextDay(Dictionary<int, long> input)
        {
            var output = new Dictionary<int, long>();

            foreach (var (key, value) in input.OrderByDescending(x => x.Key))
            {
                if (key > 0)
                    output[key - 1] = value;

                if (key == 0)
                {
                    output[8] = value;
                    if (output.ContainsKey(6))
                        output[6] += value;
                    else
                        output[6] = value;
                }
            }

            return output;
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input, int dayCount = 80)
        {
            var dictionary = ParseInitialState(input[0]);
            for (var day = 0; day < dayCount; day++) 
                dictionary = GetNextDay(dictionary);

            return dictionary.Sum(x => x.Value);
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result18 = SolvePuzzle1(input, 18);
            Assert.AreEqual((long)26, result18);
            var result80 = SolvePuzzle1(input);
            Assert.AreEqual(Results.Setup1, result80);
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
        
        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input, 256);
            Assert.AreEqual(Results.Setup2, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input, 256);
            Assert.AreEqual(Results.Puzzle2, result);
        }

        #endregion
    }
}