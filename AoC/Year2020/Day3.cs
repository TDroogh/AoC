using AoC.Inputs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020
{
    [TestClass]
    public class Day3
    {
        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadArrayInput(2020, 3, 0);
            Assert.AreEqual(7, CountTrees(input, '#', 3, 1));
        }
        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadArrayInput(2020, 3, 0);

            var r1 = CountTrees(input, '#', 1, 1);
            var r2 = CountTrees(input, '#', 3, 1);
            var r3 = CountTrees(input, '#', 5, 1);
            var r4 = CountTrees(input, '#', 7, 1);
            var r5 = CountTrees(input, '#', 1, 2);

            Assert.AreEqual(336, r1 * r2 * r3 * r4 * r5);
        }

        private int CountTrees(char[,] field, char treeChar, int right, int down)
        {
            var x = right;
            var y = down;

            var treeCount = 0;
            do
            {
                if (field[x, y] == treeChar)
                    treeCount++;

                x += right;
                y += down;
            } while (y < field.GetLength(1));

            return treeCount;
        }
        
        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadArrayInput(2020, 3, 1, 2);
            Assert.AreEqual(145, CountTrees(input, '#', 3, 1));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadArrayInput(2020, 3, 1, 5);

            long r1 = CountTrees(input, '#', 1, 1);
            long r2 = CountTrees(input, '#', 3, 1);
            long r3 = CountTrees(input, '#', 5, 1);
            long r4 = CountTrees(input, '#', 7, 1);
            long r5 = CountTrees(input, '#', 1, 2);

            Assert.AreEqual(3424528800, r1 * r2 * r3 * r4 * r5);
        }
    }
}