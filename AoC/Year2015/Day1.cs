using AoC.Inputs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2015
{
    [TestClass]
    public class Day1
    {
        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput(2015, 1, 1);
            var line = input[0];

            var floor = 0;
            foreach(var chr in line)
            {
                if (chr == '(') floor++;
                else if (chr == ')') floor--;
            }

            Assert.AreEqual(280, floor);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput(2015, 1, 1);
            var line = input[0];

            var floor = 0;
            var i = 0;
            foreach(var chr in line)
            {
                i++;
                if (chr == '(') floor++;
                else if (chr == ')') floor--;
                if (floor == -1)
                    break;
            }

            Assert.AreEqual(1797, i);
        }
    }
}
