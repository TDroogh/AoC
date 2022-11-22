using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2020.Day14
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private long SolvePuzzle1(string[] input)
        {
            var mask = "".PadRight(36, 'x');
            var dict = new Dictionary<string, long>();

            foreach (var line in input)
            {
                var split = line.Split("=");
                if (line.StartsWith("mask", StringComparison.Ordinal))
                {
                    mask = split[1].Trim();
                }
                else
                {
                    var mem = split[0].Split('[', ']')[1];
                    var memValue = long.Parse(split[1].Trim());
                    long value = 0;

                    for (var i = 0; i < 36; i++)
                    {
                        var j = 35 - i;

                        var maskValue = mask[j];
                        var exp = (long)Math.Pow(2, i);

                        if (maskValue == 'x' || maskValue == 'X')
                        {
                            if (memValue % exp == 0 && memValue / exp % 2 != 0)
                            {
                                memValue -= exp;
                                value += exp;
                            }
                        }
                        else
                        {
                            if (maskValue == '1')
                                value += exp;
                            if (memValue % exp == 0 && memValue / exp % 2 != 0)
                                memValue -= exp;
                        }
                    }

                    dict[mem] = value;
                }
            }

            return dict.Values.Sum();
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(165, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(15172047086292, result);
        }

        #endregion

        #region Puzzle 2

        private long SolvePuzzle2(string[] input)
        {
            var mask = "".PadRight(36, 'x');
            var dict = new Dictionary<long, long>();

            foreach (var line in input)
            {
                var split = line.Split("=");
                if (line.StartsWith("mask", StringComparison.Ordinal))
                {
                    mask = split[1].Trim();
                }
                else
                {
                    var memValue = long.Parse(split[0].Split('[', ']')[1]);
                    var mem = long.Parse(split[1].Trim());
                    var resultMask = "".PadRight(36, '0').ToCharArray();

                    for (var i = 0; i < 36; i++)
                    {
                        var j = 35 - i;

                        var maskValue = mask[j];
                        var exp = (long)Math.Pow(2, i);

                        if (maskValue == 'x' || maskValue == 'X')
                        {
                            if (memValue % exp == 0 && memValue / exp % 2 != 0)
                                memValue -= exp;
                            resultMask[j] = 'x';
                        }
                        else
                        {
                            if (maskValue == '1')
                                resultMask[j] = '1';
                            if (memValue % exp == 0 && memValue / exp % 2 != 0)
                            {
                                resultMask[j] = '1';
                                memValue -= exp;
                            }
                        }
                    }

                    foreach (var addresses in GetAddresses(resultMask))
                        dict[addresses] = mem;
                }
            }

            return dict.Values.Sum();
        }

        private IEnumerable<long> GetAddresses(char[] addressMask)
        {
            var allMasks = new List<char[]> { addressMask };

            for (var i = 0; i < 36; i++)
            {
                var j = 35 - i;
                var maskValue = addressMask[j];

                if (maskValue == 'x')
                {
                    var newMasks = new List<char[]>();
                    foreach (var mask in allMasks)
                    {
                        var firstMask = new char[36];
                        mask.CopyTo(firstMask, 0);

                        var secondMask = new char[36];
                        mask.CopyTo(secondMask, 0);

                        firstMask[j] = '0';
                        secondMask[j] = '1';

                        newMasks.Add(firstMask);
                        newMasks.Add(secondMask);
                    }

                    allMasks = newMasks;
                }
            }

            foreach (var mask in allMasks)
            {
                long sum = 0;

                for (var i = 0; i < 36; i++)
                {
                    var j = 35 - i;

                    var maskValue = mask[j];
                    var exp = (long)Math.Pow(2, i);

                    if (maskValue == '1')
                        sum += exp;
                }

                yield return sum;
            }
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput("setup2");
            var result = SolvePuzzle2(input);
            Assert.AreEqual(208, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(4197941339968, result);
        }

        #endregion
    }
}