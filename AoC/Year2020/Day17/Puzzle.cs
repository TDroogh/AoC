using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace AoC.Year2020.Day17
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private int SolvePuzzle1(char[,] input, int cycles)
        {
            var space = new bool[input.GetLength(0), input.GetLength(1), 1];
            for (var i = 0; i < input.GetLength(0); i++)
            {
                for (var j = 0; j < input.GetLength(1); j++)
                    space[i, j, 0] = input[i, j] == '#';
            }

            Trace.WriteLine("");
            Trace.WriteLine("Start");
            PrintSpace(space);

            for (var l = 1; l <= cycles; l++)
            {
                var newSpace = new bool[space.GetLength(0) + 2, space.GetLength(1) + 2, space.GetLength(2) + 2];

                for (var i = -1; i < space.GetLength(0) + 1; i++)
                {
                    for (var j = -1; j < space.GetLength(1) + 1; j++)
                    {
                        for (var k = -1; k < space.GetLength(2) + 1; k++)
                        {
                            var currentState = false;
                            if (i >= 0 && i < space.GetLength(0) && j >= 0 && j < space.GetLength(1) && k >= 0 && k < space.GetLength(2))
                                currentState = space[i, j, k];
                            newSpace[i + 1, j + 1, k + 1] = CalculateState(space, i, j, k, currentState);
                        }
                    }
                }

                space = newSpace;

                Trace.WriteLine("");
                Trace.WriteLine($"Iteration {l}");
                PrintSpace(space);
            }

            var sum = 0;
            foreach (var x in space)
                if (x)
                    sum++;
            return sum;
        }

        private void PrintSpace(bool[,,] space)
        {
            var zBase = (space.GetLength(2) - 1) / 2;
            for (var z = 0; z < space.GetLength(2); z++)
            {
                Trace.WriteLine("------------------------");
                Trace.WriteLine($"Z = {z - zBase}");

                for (var y = 0; y < space.GetLength(1); y++)
                {
                    for (var x = 0; x < space.GetLength(0); x++)
                    {
                        Trace.Write(space[x, y, z] ? "#" : ".");
                    }

                    Trace.WriteLine("");
                }
            }
        }

        private bool CalculateState(bool[,,] space, int x, int y, int z, bool initialState)
        {
            var active = 0;
            var evaluated = 0;

            for (var i = x - 1; i <= x + 1; i++)
            {
                if (i >= 0 && i < space.GetLength(0))
                {
                    for (var j = y - 1; j <= y + 1; j++)
                    {
                        if (j >= 0 && j < space.GetLength(1))
                        {
                            for (var k = z - 1; k <= z + 1; k++)
                            {
                                if (k >= 0 && k < space.GetLength(2))
                                {
                                    if (x == i && y == j && z == k)
                                        continue;

                                    if (space[i, j, k])
                                        active++;
                                    evaluated++;
                                }
                            }
                        }
                    }
                }
            }

            Assert.IsTrue(evaluated > 0);

            return active == 3 || (initialState && active == 2);
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadArrayInput();
            Assert.AreEqual(5, SolvePuzzle1(input, 0));
            Assert.AreEqual(11, SolvePuzzle1(input, 1));
            Assert.AreEqual(21, SolvePuzzle1(input, 2));
            Assert.AreEqual(38, SolvePuzzle1(input, 3));
            Assert.AreEqual(112, SolvePuzzle1(input, 6));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle1(input, 6);
            Assert.AreEqual(346, result);
        }

        #endregion

        #region Puzzle 2

        private int SolvePuzzle2(char[,] input, int cycles)
        {
            var space = new bool[input.GetLength(0), input.GetLength(1), 1, 1];
            for (var i = 0; i < input.GetLength(0); i++)
            {
                for (var j = 0; j < input.GetLength(1); j++)
                    space[i, j, 0, 0] = input[i, j] == '#';
            }

            Trace.WriteLine("");
            Trace.WriteLine("Start");
            //PrintSpace2(space);

            for (var l = 1; l <= cycles; l++)
            {
                var newSpace = new bool[space.GetLength(0) + 2, space.GetLength(1) + 2, space.GetLength(2) + 2, space.GetLength(3) + 2];

                for (var i = -1; i < space.GetLength(0) + 1; i++)
                {
                    for (var j = -1; j < space.GetLength(1) + 1; j++)
                    {
                        for (var k = -1; k < space.GetLength(2) + 1; k++)
                        {
                            for (var m = -1; m < space.GetLength(3) + 1; m++)
                            {
                                var currentState = false;
                                if (i >= 0 && i < space.GetLength(0) && j >= 0 && j < space.GetLength(1) && k >= 0 && k < space.GetLength(2) && m >= 0 && m < space.GetLength(3))
                                    currentState = space[i, j, k, m];
                                newSpace[i + 1, j + 1, k + 1, m + 1] = CalculateState2(space, i, j, k, m, currentState);
                            }
                        }
                    }
                }

                space = newSpace;

                Trace.WriteLine("");
                Trace.WriteLine($"Iteration {l}");
                //PrintSpace2(space);
            }

            var sum = 0;
            foreach (var x in space)
                if (x)
                    sum++;
            return sum;
        }

        //private void PrintSpace2(bool[,,,] space)
        //{
        //    //var zBase = (space.GetLength(2) - 1) / 2;
        //    //for (var z = 0; z < space.GetLength(2); z++)
        //    //{
        //    //    Trace.WriteLine("------------------------");
        //    //    Trace.WriteLine($"Z = {z - zBase}");

        //    //    for (var y = 0; y < space.GetLength(1); y++)
        //    //    {
        //    //        for (var x = 0; x < space.GetLength(0); x++)
        //    //        {
        //    //            Trace.Write(space[x, y, z] ? "#" : ".");
        //    //        }

        //    //        Trace.WriteLine("");
        //    //    }
        //    //}
        //}

        private bool CalculateState2(bool[,,,] space, int x, int y, int z, int a, bool initialState)
        {
            var active = 0;
            var evaluated = 0;

            for (var i = x - 1; i <= x + 1; i++)
            {
                if (i >= 0 && i < space.GetLength(0))
                {
                    for (var j = y - 1; j <= y + 1; j++)
                    {
                        if (j >= 0 && j < space.GetLength(1))
                        {
                            for (var k = z - 1; k <= z + 1; k++)
                            {
                                if (k >= 0 && k < space.GetLength(2))
                                {
                                    for (var m = a - 1; m <= a + 1; m++)
                                    {
                                        if (m >= 0 && m < space.GetLength(3))
                                        {
                                            if (x == i && y == j && z == k && a == m)
                                                continue;

                                            if (space[i, j, k, m])
                                                active++;
                                            evaluated++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Assert.IsTrue(evaluated > 0);

            return active == 3 || (initialState && active == 2);
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadArrayInput();
            Assert.AreEqual(5, SolvePuzzle2(input, 0));
            //Assert.AreEqual(11, SolvePuzzle2(input, 1));
            //Assert.AreEqual(21, SolvePuzzle2(input, 2));
            //Assert.AreEqual(38, SolvePuzzle2(input, 3));
            Assert.AreEqual(848, SolvePuzzle2(input, 6));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle2(input, 6);
            Assert.AreEqual(1632, result);
        }

        #endregion
    }
}