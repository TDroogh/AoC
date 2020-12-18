using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020.Day18
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private long SolvePuzzle(string[] input, bool includeOrder)
        {
            long totalSum = 0;
            foreach (var line in input)
                totalSum += SolveLine(line, includeOrder);
            return totalSum;
        }

        private long SolveLine(string line, bool includeOrder)
        {
            while (line.Contains('('))
            {
                var open = line.IndexOf('(');
                var close = -1;
                var depth = 0;
                var totalLine = "(";
                for (var i = open + 1; i < line.Length; i++)
                {
                    var nextChar = line[i];
                    totalLine += nextChar;

                    if (nextChar == '(')
                        depth++;
                    else if (nextChar == ')')
                    {
                        if (depth == 0)
                        {
                            close = i;
                            break;
                        }

                        depth--;
                    }
                }

                var subLine = line.Substring(open + 1, close - open - 1);
                var subResult = SolveLine(subLine, includeOrder).ToString();
                line = line.Replace(totalLine, subResult);
            }

            if (includeOrder)
            {
                while (line.Contains("+"))
                {
                    var terms = line.Split(" ");
                    for (var i = 1; i < terms.Length - 1; i += 2)
                    {
                        var @operator = terms[i];
                        if (@operator == "+")
                        {
                            var first = long.Parse(terms[i - 1]);
                            var second = long.Parse(terms[i + 1]);

                            var equation = $"{first} + {second}";
                            var result = SolveWithoutOperands(equation);

                            line = line.Replace(equation, result.ToString());
                            break;
                        }
                    }
                }
            }

            return SolveWithoutOperands(line);
        }

        private static long SolveWithoutOperands(string line)
        {
            var terms = line.Split(" ");
            var result = long.Parse(terms[0]);
            for (var i = 1; i < terms.Length - 1; i += 2)
            {
                var @operator = terms[i];
                var operand = long.Parse(terms[i + 1]);

                if (@operator == "*")
                    result *= operand;
                if (@operator == "+")
                    result += operand;
            }

            return result;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(71, SolveLine(input[4], false));
            Assert.AreEqual(26, SolveLine(input[0], false));
            Assert.AreEqual(437, SolveLine(input[1], false));
            Assert.AreEqual(12240, SolveLine(input[2], false));
            Assert.AreEqual(13632, SolveLine(input[3], false));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle(input, false);
            Assert.AreEqual(6811433855019, result);
        }

        #endregion

        #region Puzzle 2

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(231, SolveLine(input[4], true));
            Assert.AreEqual(46, SolveLine(input[0], true));
            Assert.AreEqual(1445, SolveLine(input[1], true));
            Assert.AreEqual(669060, SolveLine(input[2], true));
            Assert.AreEqual(23340, SolveLine(input[3], true));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle(input, true);
            Assert.AreEqual(129770152447927, result);
        }

        #endregion
    }
}