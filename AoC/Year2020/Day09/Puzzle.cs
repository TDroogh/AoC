using System.Collections.Generic;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020.Day09
{
    [TestClass]
    public class Puzzle
    {
        private long FindFirstOutlier(long[] input, int preamble)
        {
            for (var i = preamble; i < input.Length; i++)
            {
                var current = input[i];
                var foundSum = false;
                for (var j = i - preamble; j <= i && !foundSum; j++)
                {
                    for (var k = j + 1; k <= i && !foundSum; k++)
                    {
                        if (input[j] + input[k] == current)
                            foundSum = true;
                    }
                }

                if (!foundSum)
                    return current;
            }

            return 0;
        }

        private long[] FindContiguousSet(long[] input, long sum)
        {
            for (var i = 0; i < input.Length; i++)
            {
                var list = new List<long> { input[i] };

                for (var j = i + 1; j < input.Length; j++)
                {
                    list.Add(input[j]);
                    var listSum = list.Sum();
                    if (listSum > sum)
                        break;
                    if (listSum == sum)
                        return list.ToArray();
                }
            }

            return new long[0];
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadLongInput();
            Assert.AreEqual(127, FindFirstOutlier(input, 5));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadLongInput();
            Assert.AreEqual(36845998, FindFirstOutlier(input, 25));
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadLongInput();
            var set = FindContiguousSet(input, 127);

            Assert.AreEqual(62, set.Min() + set.Max());
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadLongInput();
            var set = FindContiguousSet(input, 36845998);

            Assert.AreEqual(4830226, set.Min() + set.Max());
        }
    }
}
