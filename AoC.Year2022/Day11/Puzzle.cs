using System.Numerics;
using Xunit.Abstractions;

namespace AoC.Year2022.Day11
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
            public const long Setup1 = 10605;
            public const long Puzzle1 = 56350;
            public const long Setup2 = 2713310158;
            public const long Puzzle2 = 13954061248;
        }

        public class Monkey
        {
            public required List<BigInteger> StartingItems { get; init; }
            public required string Operation { get; init; }
            public required int TestDivisibleBy { get; init; }
            public required int IfTrueThrowTo { get; init; }
            public required int IfFalseThrowTo { get; init; }
            public required long TotalCount { get; set; }

            public BigInteger ExecuteOperation(BigInteger old)
            {
                TotalCount++;
                //var operation = Operation.Replace("old", old.ToString());

                return Operation.Contains('+')
                    ? Operation.Split('+').Select(val => GetBigInt(val, old)).Sum()
                    : Operation.Split('*').Select(val => GetBigInt(val, old)).Product();
            }

            public bool TestDivisible(BigInteger value)
            {
                if (TestDivisibleBy == 2)
                {
                    var lastNumber = value.ToString()[^1];
                    return lastNumber is '2' or '4' or '6' or '8' or '0';
                }

                if (TestDivisibleBy == 5)
                {
                    var lastNumber = value.ToString()[^1];
                    return lastNumber is '5' or '0';
                }

                return value % TestDivisibleBy == 0;
            }

            private BigInteger GetBigInt(string value, BigInteger old)
            {
                return value.Trim() switch
                {
                    "7" => 7,
                    "19" => 19,
                    "17" => 17,
                    "2" => 2,
                    "1" => 1,
                    "6" => 6,
                    "3" => 3,
                    "4" => 4,
                    "old" => old,
                    _ => BigInteger.Parse(value)
                };
            }

            public static Monkey Parse(string[] lines)
            {
                return new Monkey
                {
                    StartingItems = lines[1].Split(':')[1].Split(',').Select(BigInteger.Parse).ToList(),
                    Operation = lines[2][19..],
                    TestDivisibleBy = int.Parse(lines[3][21..]),
                    IfTrueThrowTo = int.Parse(lines[4][29..]),
                    IfFalseThrowTo = int.Parse(lines[5][30..]),
                    TotalCount = 0
                };
            }
        }

        private Dictionary<int, Monkey> ParseMonkeys(string[] input)
        {
            var monkeys = new Dictionary<int, Monkey>();

            for (var i = 0; i < input.Length; i += 7)
            {
                var monkeyIndex = i / 7;
                var lines = input.Skip(i).Take(7).ToArray();
                var monkey = Monkey.Parse(lines);
                monkeys.Add(monkeyIndex, monkey);

                _helper.WriteLine($"Monkey {monkeyIndex}");
                _helper.WriteLine($"Starting items {string.Join(',', monkey.StartingItems)}");
                _helper.WriteLine($"Operation {monkey.Operation}");
                _helper.WriteLine($"Test: Divisible by {monkey.TestDivisibleBy}");
                _helper.WriteLine($"If true {monkey.IfTrueThrowTo}");
                _helper.WriteLine($"If false {monkey.IfFalseThrowTo}");
                _helper.WriteLine("------------------");
            }

            return monkeys;
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var monkeys = ParseMonkeys(input);

            for (var r = 1; r <= 20; r++)
            {
                foreach (var monkey in monkeys.Values)
                {
                    foreach (var startingItem in monkey.StartingItems.ToList())
                    {
                        monkey.StartingItems.Remove(startingItem);
                        var worryLevel = monkey.ExecuteOperation(startingItem) / 3;
                        var monkeyToThrowTo = monkey.TestDivisible(worryLevel) ? monkey.IfTrueThrowTo : monkey.IfFalseThrowTo;
                        monkeys[monkeyToThrowTo].StartingItems.Add(worryLevel);
                    }
                }

                if (r % 1 == 0)
                {
                    _helper.WriteLine($"After round {r}:");
                    foreach (var kvp in monkeys)
                    {
                        _helper.WriteLine($"Monkey {kvp.Key}, {string.Join(',', kvp.Value.StartingItems)} (total count {kvp.Value.TotalCount})");
                    }

                    _helper.WriteLine("------------------");
                }
            }

            return monkeys.Values.OrderByDescending(m => m.TotalCount).Take(2).Product(m => m.TotalCount);
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
            var monkeys = ParseMonkeys(input);
            var mod = monkeys.Product(m => m.Value.TestDivisibleBy);
            for (var r = 1; r <= 10000; r++)
            {
                foreach (var monkey in monkeys.Values)
                {
                    foreach (var startingItem in monkey.StartingItems.ToList())
                    {
                        monkey.StartingItems.Remove(startingItem);
                        var worryLevel = monkey.ExecuteOperation(startingItem) % mod;
                        var monkeyToThrowTo = monkey.TestDivisible(worryLevel) ? monkey.IfTrueThrowTo : monkey.IfFalseThrowTo;
                        monkeys[monkeyToThrowTo].StartingItems.Add(worryLevel);
                    }
                }

                if (r % 100 == 0)
                {
                    _helper.WriteLine($"After round {r}:");
                    foreach (var kvp in monkeys)
                    {
                        _helper.WriteLine($"Monkey {kvp.Key} (total count {kvp.Value.TotalCount})");
                    }

                    _helper.WriteLine("------------------");
                }
            }

            return monkeys.Values.OrderByDescending(m => m.TotalCount).Take(2).Product(m => m.TotalCount);
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
