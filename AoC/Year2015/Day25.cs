using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2015
{
    [TestClass]
    public class Day25
    {
        private long CalculateNext(long input)
        {
            var result = input * 252533;
            return result % 33554393;
        }

        [TestMethod]
        public void Setup1()
        {
            var i1 = 20151125;
            var i2 = 31916031;
            var i3 = 18749137;
            var i4 = 16080970;

            Assert.AreEqual(i2, CalculateNext(i1));
            Assert.AreEqual(i3, CalculateNext(i2));
            Assert.AreEqual(i4, CalculateNext(i3));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var row = 1;
            var col = 1;
            long input = 20151125;

            while (!(row == 2947 && col == 3029))
            {
                input = CalculateNext(input);

                if (row == 1)
                {
                    row = col + 1;
                    col = 1;
                }
                else
                {
                    row--;
                    col++;
                }
            }
            
            Assert.AreEqual(19980801, input);
        }
    }
}
