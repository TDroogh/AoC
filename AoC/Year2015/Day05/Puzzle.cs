using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AoC.Year2015.Day05
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            return input.Count(IsNice1);
        }

        private bool IsNice1(string line)
        {
            const string vowels = "aeiou";
            if (line.Count(x => vowels.Contains(x)) < 3)
                return false;

            var prev = '\0';
            if (line.Any(x =>
                {
                    var result = x == prev;
                    prev = x;
                    return result;
                }) == false)
                return false;

            var forbiddenStrings = new[] { "ab", "cd", "pq", "xy" };
            for (var i = 0; i < line.Length - 1; i++)
            {
                var str = line[i].ToString() + line[i + 1];
                if (forbiddenStrings.Contains(str))
                    return false;
            }

            return true;
        }

        [TestMethod]
        public void Setup1()
        {
            Assert.AreEqual(true, IsNice1("ugknbfddgicrmopn"));
            Assert.AreEqual(true, IsNice1("aaa"));
            Assert.AreEqual(false, IsNice1("jchzalrnumimnmhp"));
            Assert.AreEqual(false, IsNice1("haegwjzuvuyypxyu"));
            Assert.AreEqual(false, IsNice1("dvszwmarrgswjxmb"));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(255, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input)
        {
            return input.Count(IsNice2);
        }

        private bool IsNice2(string line)
        {
            var anyDoubles = false;
            for (var i = 0; i < line.Length - 3; i++)
            {
                var str = line[i].ToString() + line[i + 1];
                if (line.Substring(i + 2).Contains(str))
                    anyDoubles = true;
            }

            if (!anyDoubles)
                return false;

            var anyRepeat = false;
            for (var i = 0; i < line.Length - 2; i++)
            {
                if (line[i] == line[i + 2])
                    anyRepeat = true;
            }

            return anyRepeat;
        }

        [TestMethod]
        public void Setup2()
        {
            Assert.AreEqual(true, IsNice2("qjhvhtzxzqqjkmpb"));
            Assert.AreEqual(true, IsNice2("xxyxx"));
            Assert.AreEqual(false, IsNice2("uurcxstgmygtbstg"));
            Assert.AreEqual(false, IsNice2("ieodomkazucvgmuy"));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(55, result);
        }

        #endregion
    }
}