using System;
using System.Diagnostics;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day18
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup11 = 1137;
            public const int Setup12 = 3488;
            public const int Setup13 = 4140;
            public const int Puzzle1 = 3647;
            public const int Setup2 = 3993;
            public const int Puzzle2 = 4600;
        }

        public abstract class Sum
        {
            public abstract int GetMagnitude();

            public static Sum Parse(string input)
            {
                return input[0] == '['
                    ? Pair.ParsePair(input)
                    : Literal.ParseLiteral(input);
            }
        }

        public class Pair : Sum
        {
            public Pair(Sum left, Sum right)
            {
                Left = left;
                Right = right;
            }

            public Sum Left { get; set; }
            public Sum Right { get; set; }

            /// <inheritdoc />
            public override int GetMagnitude()
            {
                var left = Left.GetMagnitude() * 3;
                var right = Right.GetMagnitude() * 2;

                return left + right;
            }

            public static Pair ParsePair(string input)
            {
                var separatorIndex = -1;
                var nestingIndex = 0;
                for (var i = 1; i < input.Length - 1; i++)
                {
                    var chr = input[i];
                    if (chr == '[')
                        nestingIndex++;
                    if (chr == ']')
                        nestingIndex--;
                    if (chr == ',' && nestingIndex == 0)
                    {
                        separatorIndex = i;
                        break;
                    }
                }

                Debug.Assert(separatorIndex != -1);

                var left = input[1..separatorIndex];
                var right = input[(separatorIndex + 1)..^1];

                return new Pair(Parse(left), Parse(right));
            }

            public override string ToString()
            {
                return $"[{Left},{Right}]";
            }
        }

        public class Literal : Sum
        {
            public Literal(int value)
            {
                Value = value;
            }

            public int Value { get; set; }

            /// <inheritdoc />
            public override int GetMagnitude()
            {
                return Value;
            }

            public static Literal ParseLiteral(string input)
            {
                return new Literal(int.Parse(input));
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        private static string Add(string left, string right)
        {
            Console.WriteLine($"Adding {left} and {right}");
            var pair = $"[{left},{right}]";

            var i = 0;
            Console.WriteLine($"Before reduction   : {pair}");
            while (TryReduce(pair, out var newPair) && i < 10_000)
            {
                pair = newPair;
                Console.WriteLine($"After reduction {i++:D3}: {pair}");
            }

            Debug.Assert(i < 10_000);

            return pair;
        }

        private static bool TryReduce(string input, out string output)
        {
            var depth = 0;
            for (var pos = 0; pos < input.Length; pos++)
            {
                var c = input[pos];

                if (c == '[')
                {
                    depth++;
                    if (depth > 4)
                    {
                        output = Explode(input, pos);
                        return true;
                    }
                }

                if (c == ']')
                    depth--;
            }

            for (var pos = 0; pos < input.Length; pos++)
            {
                var c = input[pos];

                if (char.IsDigit(c) && char.IsDigit(input[pos + 1]))
                {
                    output = Split(input, pos);
                    return true;
                }
            }

            output = input;
            return false;
        }

        private static string Split(string str, int pos)
        {
            var stringValue = str[pos].ToString();
            for (var j = pos + 1; char.IsDigit(str[j]); j++)
                stringValue += str[j];

            var value = int.Parse(stringValue);
            Console.WriteLine($"Splitting {value}");

            var left = value / 2;
            var right = (value + 1) / 2;
            var replacement = $"[{left},{right}]";

            return str.ReplaceFirst(stringValue, replacement);
        }

        private static string Explode(string str, int pos)
        {
            var value = "";
            for (var i = pos; i < str.Length; i++)
            {
                value += str[i];
                if (str[i] == ']')
                    break;
            }
            
            Console.WriteLine($"Exploding {value}");
            var values = value[1..^1].Split(",");

            var newString = str[..pos] + 0 + str[(pos + value.Length)..];

            //Do the right part first so we don't potentially shift positions
            var right = int.Parse(values[1]);
            Debug.Assert(newString[pos] == '0');
            for (var i = pos + 1; i < newString.Length; i++)
            {
                if (char.IsDigit(newString[i]))
                {
                    var stringVal = newString[i].ToString();
                    for (var j = i + 1; char.IsDigit(newString[j]); j++)
                        stringVal += newString[j];

                    var val = int.Parse(stringVal);
                    var total = val + right;

                    Console.WriteLine($"Exploding {value} to right: {val} + {right} = {total}");
                    newString = newString[..i] + total + newString[(i + stringVal.Length)..];
                    break;
                }
            }

            var left = int.Parse(values[0]);
            Debug.Assert(newString[pos] == '0');
            for (var i = pos - 1; i >= 0; i--)
            {
                if (char.IsDigit(newString[i]))
                {
                    var stringVal = newString[i].ToString();
                    for (var j = i - 1; char.IsDigit(newString[j]); j--)
                        stringVal = newString[j] + stringVal;

                    var val = int.Parse(stringVal);
                    var total = val + left;

                    Console.WriteLine($"Exploding {value} to left: {val} + {left} = {total}");
                    newString = newString[..(i + 1 - stringVal.Length)] + total + newString[(i + 1)..];
                    break;
                }
            }

            Console.WriteLine($">> Before: {str}");
            Console.WriteLine($">> After : {newString}");
            //Console.WriteLine($"Exploded {value}: {newString}");
            return newString;
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var sum = input[0];

            foreach (var nextSum in input.Skip(1))
                sum = Add(sum, nextSum);

            return Sum.Parse(sum).GetMagnitude();
        }

        [DataTestMethod]
        [DataRow("[1,2]", "[[3,4],5]", "[[1,2],[[3,4],5]]")]
        [DataRow("[1,2]", "[1,10]", "[[1,2],[1,[5,5]]]")]
        [DataRow("[[[[9,8],1],2],3]", "4", "[[[[0,9],2],3],4]")]
        [DataRow("[[[[4,3],4],4],[7,[[8,4],9]]]", "[1,1]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
        //Example from the page
        [DataRow("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]", "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]", "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]", DisplayName = "Step 1")]
        [DataRow("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]", "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]", "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]", DisplayName = "Step 2")]
        [DataRow("[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]", "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]", "[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]", DisplayName = "Step 3")]
        [DataRow("[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]", "[7,[5,[[3,8],[1,4]]]]", "[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]", DisplayName = "Step 4")]
        [DataRow("[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]", "[[2,[2,2]],[8,[8,1]]]", "[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]", DisplayName = "Step 5")]
        [DataRow("[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]", "[2,9]", "[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]", DisplayName = "Step 6")]
        [DataRow("[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]", "[1,[[[9,3],9],[[9,0],[0,7]]]]", "[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]", DisplayName = "Step 7")]
        [DataRow("[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]", "[[[5,[7,4]],7],1]", "[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]", DisplayName = "Step 8")]
        [DataRow("[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]", "[[[[4,2],2],6],[8,7]]", "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", DisplayName = "Step 9")]
        public void Add_HappyFlow(string left, string right, string expectedResult)
        {
            var result = Add(left, right);
            Assert.AreEqual(expectedResult, result);
        }

        [DataTestMethod]
        [DataRow("[9,1]", 29)]
        [DataRow("[1,9]", 21)]
        [DataRow("[[9,1],[1,9]]", 129)]
        [DataRow("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137)]
        [DataRow("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488)]
        public void Magnitude_HappyFlow(string input, int expectedOutput)
        {
            var magnitude = Sum.Parse(input).GetMagnitude();
            Assert.AreEqual(expectedOutput, magnitude);
        }

        [DataTestMethod]
        [DataRow("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
        [DataRow("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
        [DataRow("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
        [DataRow("[[13,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[13,[2,[8,0]]],[9,[5,[4,[3,2]]]]]")]
        [DataRow("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
        public void Reduce_HappyFlow(string input, string expectedOutput)
        {
            TryReduce(input, out var output);
            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void Setup11()
        {
            var input = InputReader.ReadInput("setup1");
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Setup11, result);
        }

        [TestMethod]
        public void Setup12()
        {
            var input = InputReader.ReadInput("setup2");
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Setup12, result);
        }

        [TestMethod]
        public void Setup13()
        {
            var input = InputReader.ReadInput("setup3");
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Setup13, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input)
        {
            var max = -1;
            for (var i = 0; i < input.Length - 1; i++)
            {
                for (var j = i + 1; j < input.Length; j++)
                {
                    var l = input[i];
                    var r = input[j];

                    var sum = Sum.Parse(Add(l, r)).GetMagnitude();
                    if (sum > max)
                        max = sum;

                    sum = Sum.Parse(Add(r, l)).GetMagnitude();
                    if (sum > max)                        
                        max = sum;
                }
            }

            return max;
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput("setup3");
            var result = SolvePuzzle2(input);
            Assert.AreEqual(Results.Setup2, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(Results.Puzzle2, result);
        }

        #endregion
    }
}