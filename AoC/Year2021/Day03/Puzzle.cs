using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AoC.Year2021.Day03
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 198;
            public const int Puzzle1 = 3_277_364;
            public const int Setup2 = 230;
            public const int Puzzle2 = 5_736_383;
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var length = input[0].Length;
            var gammaRate = 0;
            var epsilonRate = 0;

            for (var i = 0; i < length; i++)
            {
                var zeroCount = 0;
                var oneCount = 0;

                foreach (var chr in input.Select(x => x[i]))
                {
                    if (chr == '0')
                        zeroCount++;
                    else if (chr == '1')
                        oneCount++;
                }

                if (oneCount > zeroCount)
                    gammaRate += 1 << length - i - 1;
                else
                    epsilonRate += 1 << length - i - 1;
            }

            return gammaRate * epsilonRate;
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
            var co2ScrubberRating = GetCo2Rating(input);
            var oxygenGeneratorRating = GetOxygenRating(input);

            return oxygenGeneratorRating * co2ScrubberRating;
        }

        private static int GetOxygenRating(string[] input)
        {
            var length = input[0].Length;
            var oxygenGeneratorRating = 0;
            var match = "";
            for (var i = 0; i < length; i++)
            {
                var zeroCount = 0;
                var oneCount = 0;

                foreach (var chr in input.Where(x => x.StartsWith(match, StringComparison.OrdinalIgnoreCase)).Select(x => x[i]))
                {
                    if (chr == '0')
                        zeroCount++;
                    else if (chr == '1')
                        oneCount++;
                }

                if (oneCount >= zeroCount)
                {
                    match += "1";
                    oxygenGeneratorRating += 1 << length - i - 1;
                }
                else
                {
                    match += "0";
                }
            }

            return oxygenGeneratorRating;
        }

        private static int GetCo2Rating(string[] input)
        {
            var length = input[0].Length;
            var match = "";
            var oxygenGeneratorRating = 0;
            for (var i = 0; i < length; i++)
            {
                var zeroCount = 0;
                var oneCount = 0;

                foreach (var chr in input.Where(x => x.StartsWith(match, StringComparison.OrdinalIgnoreCase)).Select(x => x[i]))
                {
                    if (chr == '0')
                        zeroCount++;
                    else if (chr == '1')
                        oneCount++;
                }

                if (oneCount + zeroCount == 1)
                {
                    if (oneCount == 1)
                    {
                        match += "1";
                        oxygenGeneratorRating += 1 << length - i - 1;
                    }
                    else
                    {
                        match += "0";
                    }
                }
                else
                {
                    if (oneCount < zeroCount)
                    {
                        match += "1";
                        oxygenGeneratorRating += 1 << length - i - 1;
                    }
                    else
                    {
                        match += "0";
                    }
                }
            }

            return oxygenGeneratorRating;
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