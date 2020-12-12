using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2015.Day01
{
    [TestClass]
    public class Puzzle
    {
        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var line = input[0];

            var floor = 0;
            foreach (var chr in line)
            {
                if (chr == '(')
                    floor++;
                else if (chr == ')')
                    floor--;
            }

            Assert.AreEqual(280, floor);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var line = input[0];

            var floor = 0;
            var i = 0;
            foreach (var chr in line)
            {
                i++;
                if (chr == '(')
                    floor++;
                else if (chr == ')')
                    floor--;
                if (floor == -1)
                    break;
            }

            Assert.AreEqual(1797, i);
        }
    }
}
