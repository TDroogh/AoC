using AoC.Inputs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020
{
    [TestClass]
    public class Day1
    {
        #region input

        #endregion

        [TestMethod]
        public void Test1()
        {
            var result = GetMultiplyFor2();
            Assert.AreEqual(result, 870331);
        }

        private int GetMultiplyFor2()
        {
            var input = InputReader.ReadIntInput(2020, 1, null);
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
        public void Test2()
        {
            var result = GetMultiplyFor3();
            Assert.AreEqual(result, 283025088);
        }

        private int GetMultiplyFor3()
        {
            var input = InputReader.ReadIntInput(2020, 1, null);
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
