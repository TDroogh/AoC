using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2015.Day04
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private long SolvePuzzle1(string input, int zeroes = 5)
        {
            var alg = MD5.Create();
            for (var i = 0; i < uint.MaxValue; i++)
            {
                var text = input + i;
                var bytes = Encoding.UTF8.GetBytes(text);
                var hash = alg.ComputeHash(bytes);

                if (hash[0] == 0 && hash[1] == 0)
                {
                    if (zeroes == 5 && hash[2] < 16)
                        return i;
                    if (zeroes == 6 && hash[2] == 0)
                        return i;
                }
            }

            return -1;
        }

        [TestMethod]
        public void Setup1()
        {
            Assert.AreEqual(609043, SolvePuzzle1("abcdef"));
            Assert.AreEqual(1048970, SolvePuzzle1("pqrstuv"));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var result = SolvePuzzle1("bgvyzdsv");
            Assert.AreEqual(254575, result);
        }

        #endregion

        #region Puzzle 2

        [TestMethod]
        public void Puzzle2()
        {
            var result = SolvePuzzle1("bgvyzdsv", 6);
            Assert.AreEqual(1038736, result);
        }

        #endregion
    }
}