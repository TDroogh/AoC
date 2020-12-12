using System.Collections.Generic;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2015.Day03
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private int SolvePuzzle1(params string[] input)
        {
            var x = 0;
            var y = 0;
            var dict = new List<string> { "0,0" };

            foreach (var chr in input.SelectMany(z => z))
            {
                switch (chr)
                {
                    case '>':
                        x++;
                        break;
                    case '<':
                        x--;
                        break;
                    case 'v':
                        y--;
                        break;
                    case '^':
                        y++;
                        break;
                }
                dict.Add($"{x},{y}");
            }

            return dict.Distinct().Count();
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(2572, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(params string[] input)
        {
            var santaX = 0;
            var santaY = 0;
            var roboX = 0;
            var roboY = 0;
            var dict = new List<string> { "0,0" };
            var santa = true;
            foreach (var chr in input.SelectMany(z => z))
            {
                if (santa)
                {
                    switch (chr)
                    {
                        case '>':
                            santaX++;
                            break;
                        case '<':
                            santaX--;
                            break;
                        case 'v':
                            santaY--;
                            break;
                        case '^':
                            santaY++;
                            break;
                    }

                    dict.Add($"{santaX},{santaY}");
                }
                else
                {
                    switch (chr)
                    {
                        case '>':
                            roboX++;
                            break;
                        case '<':
                            roboX--;
                            break;
                        case 'v':
                            roboY--;
                            break;
                        case '^':
                            roboY++;
                            break;
                    }

                    dict.Add($"{roboX},{roboY}");
                }

                santa = !santa;
            }

            return dict.Distinct().Count();
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(11, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(2631, result);
        }

        #endregion
    }
}