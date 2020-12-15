using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020.Day02
{
    [TestClass]
    public class Puzzle
    {
        [TestMethod]
        public void Setup1()
        {
            VerifyPassword1("1-3 a: abcde", true);
            VerifyPassword1("1-3 b: cdefg", false);
            VerifyPassword1("2-9 c: ccccccccc", true);
        }

        private void VerifyPassword1(string input, bool expected)
        {
            var result = VerifyPassword1(input);

            Assert.AreEqual(result, expected, input);
        }

        private static bool VerifyPassword1(string input)
        {
            var policy = input.Split(":")[0];
            var policySplit = policy.Split("-");
            var minimum = int.Parse(policySplit[0]);
            var maximum = int.Parse(policySplit[1].Split(' ')[0]);
            var countChar = policySplit[1].Split(' ')[1];

            var pw = input.Split(":")[1];
            var count = pw.Count(x => x.ToString() == countChar);
            var result = count >= minimum && count <= maximum;
            return result;
        }

        [TestMethod]
        public void Setup2()
        {
            VerifyPassword2("1-3 a: abcde", true);
            VerifyPassword2("1-3 b: cdefg", false);
            VerifyPassword2("2-9 c: ccccccccc", false);
        }

        private void VerifyPassword2(string input, bool expected)
        {
            var result = VerifyPassword2(input);

            Assert.AreEqual(expected, result, input);
        }

        private static bool VerifyPassword2(string input)
        {
            var policy = input.Split(":")[0];
            var first = int.Parse(policy.Split("-")[0]) - 1;
            var second = int.Parse(policy.Split("-")[1].Split(' ')[0]) - 1;
            var countChar = policy.Split("-")[1].Split(' ')[1];

            var pw = input.Split(":")[1].Trim();
            return pw[first].ToString() == countChar != (pw[second].ToString() == countChar);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = input.Count(VerifyPassword1);

            Assert.AreEqual(result, 493);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = input.Count(VerifyPassword2);

            Assert.AreEqual(result, 593);
        }
    }
}