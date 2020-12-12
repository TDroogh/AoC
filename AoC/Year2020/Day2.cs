using System;
using System.Linq;
using AoC.Inputs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020
{
    [TestClass]
    public class Day2
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
            Console.WriteLine(input);

            var policy = input.Split(":")[0];
            var minimum = int.Parse(policy.Split("-")[0]);
            var maximum = int.Parse(policy.Split("-")[1].Split(' ')[0]);
            var countChar = policy.Split("-")[1].Split(' ')[1];

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
            Console.WriteLine(input);

            var policy = input.Split(":")[0];
            var first = int.Parse(policy.Split("-")[0]) - 1;
            var second = int.Parse(policy.Split("-")[1].Split(' ')[0]) - 1; 
            var countChar = policy.Split("-")[1].Split(' ')[1];
            
            var pw = input.Split(":")[1].Trim();
            return pw[first].ToString() == countChar != (pw[second].ToString() == countChar);
        }
        
        [TestMethod]
        public void Test1()
        {
            var input = InputReader.ReadInput(2020, 2, null);
            var result = input.Count(VerifyPassword1);

            Assert.AreEqual(result, 493);
        }
        
        [TestMethod]
        public void Test2()
        {
            var input = InputReader.ReadInput(2020, 2, null);
            var result = input.Count(VerifyPassword2);

            Assert.AreEqual(result, 593);
        }
    }
}