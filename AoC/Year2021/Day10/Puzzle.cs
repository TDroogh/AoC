using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2021.Day10
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 26397;
            public const int Puzzle1 = 243939;
            public const long Setup2 = 288957;
            public const long Puzzle2 = 2421222841;
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var score = 0;
            var scores = new Dictionary<char, int>
            {
                {')', 3},
                {']', 57},
                {'}', 1197},
                {'>', 25137},
            };
            foreach (var line in input)
            {
                var stack = new Stack<char>();
                foreach (var chr in line)
                {
                    if ("([{<".Contains(chr))
                    {
                        stack.Push(chr);
                    }
                    else
                    {
                        var expectedChr = stack.Peek() switch
                        {
                            '(' => ')',
                            '{' => '}',
                            '[' => ']',
                            '<' => '>',
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        if (chr != expectedChr)
                        {
                            score += scores[chr];
                            break;
                        }

                        stack.Pop();
                    }
                }
            }

            return score;
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
            var results = new List<long>();
            var scores = new Dictionary<char, int>
            {
                {'(', 1},
                {'[', 2},
                {'{', 3},
                {'<', 4},
            };
            foreach (var line in input)
            {
                var stack = new Stack<char>();
                var failed = false;
                foreach (var chr in line)
                {
                    if ("([{<".Contains(chr))
                    {
                        stack.Push(chr);
                    }
                    else
                    {
                        var expectedChr = stack.Peek() switch
                        {
                            '(' => ')',
                            '{' => '}',
                            '[' => ']',
                            '<' => '>',
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        if (chr != expectedChr)
                        {
                            failed = true;
                            break;
                        }

                        stack.Pop();
                    }
                }

                if (!failed)
                {
                    long score = 0;

                    foreach (var chr in stack)
                    {
                        score *= 5;
                        score += scores[chr];
                    }

                    results.Add(score);
                }
            }

            return results.OrderBy(x => x).Skip((results.Count - 1) / 2).First();
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