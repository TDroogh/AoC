using System.Collections.Generic;
using AoC.Inputs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020
{
    [TestClass]
    public class Day8
    {
        private int CalculateAcc(int inputNr)
        {
            var input = InputReader.ReadInput(2020, 8, inputNr);

            var acc = 0;
            var handledIndices = new List<int>();
            var i = 0;

            while (i < input.Length)
            {
                if (handledIndices.Contains(i)) break;
                handledIndices.Add(i);

                var line = input[i];
                var split = line.Split(" ");

                var action = split[0];
                if (action == "nop")
                {
                    i++;
                }
                else if (action == "acc")
                {
                    i++;
                    acc += int.Parse(split[1]);
                }
                else if (action == "jmp")
                {
                    i += int.Parse(split[1]);
                }
            }

            return acc;
        }
        private int CalculateAccFixed(int inputNr)
        {
            var input = InputReader.ReadInput(2020, 8, inputNr);
            var fix = 0;

            while (true)
            {
                var acc = 0;
                var handledIndices = new List<int>();
                var i = 0; 

                while (true)
                {
                    if (i >= input.Length)
                    {
                        return acc;
                    }

                    if (handledIndices.Contains(i)) break;
                    handledIndices.Add(i);

                    var line = input[i];
                    var split = line.Split(" ");

                    var action = split[0];
                    if (i == fix)
                    {
                        if (action == "nop") action = "jmp";
                        if (action == "jmp") action = "nop";
                    }

                    if (action == "nop")
                    {
                        i++;
                    }
                    else if (action == "acc")
                    {
                        i++;
                        acc += int.Parse(split[1]);
                    }
                    else if (action == "jmp")
                    {
                        i += int.Parse(split[1]);
                    }
                }

                fix++;
            }
        }

        [TestMethod]
        public void Setup1()
        {
            Assert.AreEqual(5, CalculateAcc(0));
        }

        [TestMethod]
        public void Puzzle1()
        {
            Assert.AreEqual(1859, CalculateAcc(1));
        }

        [TestMethod]
        public void Setup2()
        {
            Assert.AreEqual(8, CalculateAccFixed(0));
        }

        [TestMethod]
        public void Puzzle2()
        {
            Assert.AreEqual(1235, CalculateAccFixed(1));
        }
    }
}
