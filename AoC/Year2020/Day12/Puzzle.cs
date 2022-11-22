using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AoC.Year2020.Day12
{
    [TestClass]
    public class Puzzle
    {
        private static int Navigate1(string[] input)
        {
            var x = 0;
            var y = 0;
            var face = 'E';

            void Move(char direction, int count)
            {
                switch (direction)
                {
                    case 'E':
                        x += count;
                        break;
                    case 'W':
                        x -= count;
                        break;
                    case 'S':
                        y -= count;
                        break;
                    case 'N':
                        y += count;
                        break;
                }
            }

            foreach (var command in input)
            {
                var order = command[0];
                var count = int.Parse(command.Substring(1));

                if (order == 'F')
                    Move(face, count);
                else if ("NESW".Contains(order))
                    Move(order, count);
                else if ("RL".Contains(order))
                {
                    switch (count % 360)
                    {
                        case 90 when order == 'R':
                        case 270 when order == 'L':
                            face = face switch
                            {
                                'N' => 'E',
                                'E' => 'S',
                                'S' => 'W',
                                'W' => 'N',
                                _ => face
                            };
                            break;
                        case 180:
                            face = face switch
                            {
                                'N' => 'S',
                                'E' => 'W',
                                'S' => 'N',
                                'W' => 'E',
                                _ => face
                            };
                            break;
                        case 270 when order == 'R':
                        case 90 when order == 'L':
                            face = face switch
                            {
                                'N' => 'W',
                                'E' => 'N',
                                'S' => 'E',
                                'W' => 'S',
                                _ => face
                            };
                            break;
                    }
                }
            }

            return Math.Abs(x) + Math.Abs(y);
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var output = Navigate1(input);
            Assert.AreEqual(25, output);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var output = Navigate1(input);
            Assert.AreEqual(1007, output);
        }

        private static int Navigate2(string[] input)
        {
            var x = 0;
            var y = 0;
            var wayX = 10;
            var wayY = 1;

            void MoveWay(char direction, int count)
            {
                switch (direction)
                {
                    case 'E':
                        wayX += count;
                        break;
                    case 'W':
                        wayX -= count;
                        break;
                    case 'S':
                        wayY -= count;
                        break;
                    case 'N':
                        wayY += count;
                        break;
                }
            }

            foreach (var command in input)
            {
                var order = command[0];
                var count = int.Parse(command.Substring(1));

                if (order == 'F')
                {
                    x += wayX * count;
                    y += wayY * count;
                }
                else if ("NESW".Contains(order))
                {
                    MoveWay(order, count);
                }
                else if ("RL".Contains(order))
                {
                    switch (count % 360)
                    {
                        case 90 when order == 'R':
                        case 270 when order == 'L':
                        {
                            var oldWayY = wayY;
                            wayY = -wayX;
                            wayX = oldWayY;
                            break;
                        }
                        case 180:
                        {
                            wayY = -wayY;
                            wayX = -wayX;
                            break;
                        }
                        case 270 when order == 'R':
                        case 90 when order == 'L':
                        {
                            var oldWayY = wayY;
                            wayY = wayX;
                            wayX = -oldWayY;
                            break;
                        }
                    }
                }
            }

            return Math.Abs(x) + Math.Abs(y);
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var output = Navigate2(input);
            Assert.AreEqual(286, output);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var output = Navigate2(input);
            Assert.AreEqual(41212, output);
        }
    }
}
