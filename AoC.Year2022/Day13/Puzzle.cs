using System.Text;
using Xunit.Abstractions;

namespace AoC.Year2022.Day13
{
    public class Puzzle
    {
        private readonly ITestOutputHelper _helper;

        public Puzzle(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        private static class Results
        {
            public const int Setup1 = 13;
            public const int Puzzle1 = 6623;
            public const int Setup2 = 140;
            public const int Puzzle2 = 23049;
        }

        public class Pair
        {
            public int Number { get; }
            public DataList Left { get; }
            public DataList Right { get; }

            private Pair(int number, string left, string right)
            {
                Number = number;
                Left = new DataList(left);
                Right = new DataList(right);
            }

            public static IEnumerable<Pair> ParseAll(string[] input)
            {
                for (var i = 0; i < input.Length; i += 3)
                {
                    yield return new Pair(i / 3 + 1, input[i], input[i + 1]);
                }
            }
        }

        public class DataList
        {
            public string Data { get; }
            public bool IsInteger { get; }

            public DataList(string data)
            {
                Data = data;
                IsInteger = data[0] != '[';
            }

            public IEnumerable<DataList> GetList()
            {
                if (IsInteger)
                {
                    yield return new DataList(Data);
                }
                else
                {
                    var depth = 0;
                    var sb = new StringBuilder();

                    foreach (var chr in Data[1..^1])
                    {
                        if (chr == ',' && depth == 0)
                        {
                            yield return new DataList(sb.ToString());
                            sb.Clear();
                        }
                        else
                        {
                            sb.Append(chr);
                            if (chr == '[')
                            {
                                depth++;
                            }
                            else if (chr == ']')
                            {
                                depth--;
                            }
                        }
                    }

                    if (sb.Length > 0)
                    {
                        yield return new DataList(sb.ToString());
                    }
                }
            }

            public int GetInteger()
            {
                return IsInteger ? int.Parse(Data) : -1;
            }

            public override string ToString()
            {
                return Data;
            }
        }

        private int Compare(DataList left, DataList right)
        {
            var comparer = new ValueComparer(_helper);

            return comparer.Compare(left, right);
        }

        private bool IsInRightOrder(Pair pair)
        {
            var value = Compare(pair.Left, pair.Right);
            _helper.WriteLine($"Pair {pair.Number} is in right order: {value}");
            return value < 0;
        }

        private class ValueComparer : IComparer<DataList>
        {
            private readonly ITestOutputHelper _helper;

            public ValueComparer(ITestOutputHelper helper)
            {
                _helper = helper;
            }

            public int Compare(DataList? left, DataList? right)
            {
                _helper.WriteLine($"Comparing {left} with {right}");

                var result = CompareInternal(left!, right!);

                _helper.WriteLine($"Comparing {left} with {right}: Result: {result}");

                return result;
            }

            private int CompareInternal(DataList left, DataList right)
            {
                if (left.IsInteger && right.IsInteger)
                {
                    var leftInt = left.GetInteger();
                    var rightInt = right.GetInteger();

                    if (leftInt == rightInt)
                    {
                        return 0;
                    }

                    return leftInt - rightInt;
                }

                var leftList = left.GetList().ToArray();
                var rightList = right.GetList().ToArray();

                for (var i = 0; i < Math.Min(leftList.Length, rightList.Length); i++)
                {
                    var compare = Compare(leftList[i], rightList[i]);

                    if (compare == 0)
                    {
                        continue;
                    }

                    return compare;
                }

                if (leftList.Length == rightList.Length)
                {
                    return 0;
                }

                return leftList.Length - rightList.Length;
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var sum = 0;

            foreach (var pair in Pair.ParseAll(input))
            {
                _helper.WriteLine($"Pair {pair.Number}");
                _helper.WriteLine($"L: {pair.Left}");
                _helper.WriteLine($"R: {pair.Right}");

                if (IsInRightOrder(pair))
                {
                    sum += pair.Number;
                }
            }

            return sum;
        }

        [Fact]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(Results.Setup1, result);
        }

        [Fact]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input)
        {
            var order = Pair.ParseAll(input)
                .SelectMany(p => new[] { p.Left, p.Right })
                .Union(new[] { new DataList("[[2]]"), new DataList("[[6]]") })
                .Order(new ValueComparer(_helper))
                .Select(dl => dl.Data)
                .ToList();

            return (order.IndexOf("[[2]]") + 1) * (order.IndexOf("[[6]]") + 1);
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.Equal(Results.Setup2, result);
        }

        [Fact]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.Equal(Results.Puzzle2, result);
        }

        #endregion
    }
}
