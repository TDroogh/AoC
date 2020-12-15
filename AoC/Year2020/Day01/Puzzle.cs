using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020.Day01
{
    [TestClass]
    public class Puzzle
    {
        #region input

        #endregion

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadIntInput();
            var result = GetMultiplyFor2(input);
            Assert.AreEqual(result, 870331);
        }

        private int GetMultiplyFor2(int[] input)
        {
            var length = input.Length;
            for (var i = 0; i < length; i++)
                for (var j = i; j < length; j++)
                {
                    if (input[i] + input[j] == 2020)
                        return input[i] * input[j];
                }

            return 0;
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadIntInput();
            var result = GetMultiplyFor3(input);
            Assert.AreEqual(result, 283025088);
        }

        private int GetMultiplyFor3(int[] input)
        {
            var length = input.Length;
            for (var i = 0; i < length; i++)
                for (var j = i; j < length; j++)
                    for (var k = j; k < length; k++)
                    {
                        if (input[i] + input[j] + input[k] == 2020)
                            return input[i] * input[j] * input[k];
                    }
            return 0;
        }
    }
}
