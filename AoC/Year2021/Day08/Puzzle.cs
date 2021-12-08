using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day08
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 26;
            public const int Puzzle1 = 392;
            public const int Setup2 = 61229;
            public const int Puzzle2 = 1_004_688;
        }

        #region Puzzle 1

        private static object SolvePuzzle1(string[] input)
        {
            var total = 0;

            foreach (var line in input)
            {
                var split = line.Split("|");
                var actual = split[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                total += actual.Count(x => x.Length is 2 or 3 or 4 or 7);
            }

            return total;
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

        private static object SolvePuzzle2(string[] input)
        {
            var total = 0;

            foreach (var line in input)
            {
                var split = line.Split("|");
                var dictionary = GetCodedValues(split[0]);

                var actual = split[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
                var str = "";
                foreach (var symbol in actual)
                    str += dictionary.Single(x => x.Key.Length == symbol.Length && x.Key.All(symbol.Contains)).Value.ToString();

                total += int.Parse(str);
            }

            return total;
        }

        private static Dictionary<string, int> GetCodedValues(string setup)
        {
            var split = setup.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            var results = new Dictionary<string, int>();

            //Initial values
            var figure1 = split.Single(x => x.Length == 2);
            var figure4 = split.Single(x => x.Length == 4);
            var figure7 = split.Single(x => x.Length == 3);
            var figure8 = split.Single(x => x.Length == 7);

            //Figure 9 is the only with length 6 and all characters in a 4
            var figure9 = split.Single(x => x.Length == 6 && figure4.All(x.Contains));
            //Figure 3 is the only with length 6 and all characters in a 1 (or 7)
            var figure3 = split.Single(x => x.Length == 5 && figure1.All(x.Contains));

            //Top line is the difference between 1 and 7
            var lineTop = figure7.Except(figure1).Single();
            //Bottom left is the difference between 8 and 9
            var lineBottomLeft = figure8.Except(figure9).Single();
            //Bottom line is the difference between 9 and 4 plus the top line
            var lineBottom = figure9.Except(figure4).Single(x => x != lineTop);
            //Middle line is the difference between 3 and 7 plus the bottom line
            var lineMiddle = figure3.Except(figure7).Single(x => x != lineBottom);

            //Figure 2 is the only with length 5 and a line bottom left
            var figure2 = split.Single(x => x.Length == 5 && x.Contains(lineBottomLeft));
            //Figure 5 is the remaining symbol with length 5
            var figure5 = split.Single(x => x.Length == 5 && x != figure2 && x != figure3);
            //Figure 6 is next to figure 9 the only symbol with length 6 and a line in the middle
            var figure6 = split.Single(x => x.Length == 6 && x != figure9 && x.Contains(lineMiddle));
            //Figure 0  is the remaining number
            var figure0 = split.Single(x => x.Length == 6 && x != figure9 && x != figure6);

            results.Add(figure0, 0);
            results.Add(figure1, 1);
            results.Add(figure2, 2);
            results.Add(figure3, 3);
            results.Add(figure4, 4);
            results.Add(figure5, 5);
            results.Add(figure6, 6);
            results.Add(figure7, 7);
            results.Add(figure8, 8);
            results.Add(figure9, 9);
            return results;
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