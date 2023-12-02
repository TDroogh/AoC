using System.Diagnostics;

namespace AoC.Year2020.Day23
{
    [TestClass]
    public class Puzzle
    {
        public class CupGame
        {
            private CupGame()
            {
                Cups = new Dictionary<int, Cup>();
            }

            public Cup? Start { get; set; }
            public Dictionary<int, Cup> Cups { get; }

            public static CupGame Parse(string input, int fill = 9)
            {
                var game = new CupGame();

                Cup? previous = null;
                for (var i = 1; i <= fill; i++)
                {
                    var value = i - 1 < input.Length ? int.Parse(input[i - 1].ToString()) : i;
                    var cup = new Cup
                    {
                        Value = value
                    };
                    game.Cups.Add(value, cup);
                    if (previous != null)
                        previous.Next = cup;
                    else
                        game.Start = cup;

                    previous = cup;
                }

                if (previous != null)
                    previous.Next = game.Start!;

                return game;
            }

            public Cup[] PickupThree(Cup after)
            {
                using var _ = new Timed("PickupThree");

                var cups = GetCupsAfter(after, 3);
                after.Next = cups[2].Next;
                return cups;
            }

            public Cup[] GetCupsAfter(Cup after, int count)
            {
                using var _ = new Timed("ValuesAfter");

                var result = new Cup[count];
                var previous = after;
                for (var i = 0; i < count; i++)
                {
                    result[i] = previous!.Next!;
                    previous = previous.Next;
                }

                return result;
            }

            public int[] GetValuesAfter(Cup after, int count)
            {
                return GetCupsAfter(after, count).Select(x => x.Value).ToArray();
            }

            public void PutDownThree(Cup[] values, Cup after)
            {
                using var _ = new Timed("PutDownThree");
                var nextValue = after.Value;
                Cup nextCup;
                while (true)
                {
                    nextValue--;
                    if (nextValue <= 0)
                        nextValue = Cups.Count;

                    nextCup = Cups[nextValue];
                    if (values.Contains(nextCup) == false)
                        break;
                }

                //var afterCup = Cups[after];
                values[^1].Next = nextCup.Next;
                nextCup.Next = values[0];
            }

            public Cup GetNext(Cup after)
            {
                using var _ = new Timed("GetNext");

                //var afterCup = Cups[after];

                return after.Next!;
            }
        }

        public class Cup
        {
            public int Value { get; set; }
            public Cup? Next { get; set; }

            /// <inheritdoc />
            public override string ToString()
            {
                return $"Cup {Value}, Next {Next?.Value}";
            }
        }

        #region Puzzle 1

        private class Timed : IDisposable
        {
            private static readonly Dictionary<string, long> Results = new Dictionary<string, long>();

            private readonly string _name;
            private readonly Stopwatch _sw;

            public Timed(string name)
            {
                _name = name;
                _sw = new Stopwatch();
                _sw.Start();
            }

            /// <inheritdoc />
            public void Dispose()
            {
                _sw.Stop();
                AddResult(_name, _sw);
            }

            public static void AddResult(string name, Stopwatch sw)
            {
                if (Results.ContainsKey(name))
                    Results[name] += sw.ElapsedMilliseconds;
                else
                    Results[name] = sw.ElapsedMilliseconds;
            }

            public static void PrintResults()
            {
                foreach (var kvp in Results)
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }

        private string SolvePuzzle1(string input, int iterations)
        {
            Console.WriteLine($"Input: {input}. Iterations: {iterations}");

            var cups = CupGame.Parse(input);
            var selected = cups.Start!;

            for (var i = 1; i <= iterations; i++)
            {
                Console.WriteLine($"{i}:{cups.GetValuesAfter(selected, 9).Aggregate("", (p, n) => $"{p} {n}")}");
                var nextThree = cups.PickupThree(selected);
                Console.WriteLine($"Selected {selected.Value}, Pick up {string.Join("", nextThree.Select(x => x.Value))}");
                //Console.WriteLine($"{i}:{cups.GetValuesAfter(selected, 9).Aggregate("", (p, n) => $"{p} {n}")}");
                cups.PutDownThree(nextThree, selected);
                Console.WriteLine($"{i}:{cups.GetValuesAfter(selected, 9).Aggregate("", (p, n) => $"{p} {n}")}");
                selected = cups.GetNext(selected);
            }

            //Console.WriteLine($"Final:{cups.Order.Aggregate("", (p, n) => $"{p}{n}")}");
            return cups.GetValuesAfter(cups.Cups[1], cups.Cups.Count - 1).Aggregate("", (p, n) => $"{p}{n}");
        }

        [TestMethod]
        public void Setup1()
        {
            Assert.AreEqual(SolvePuzzle1("389125467", 1), "54673289");
            Assert.AreEqual(SolvePuzzle1("389125467", 3), "34672589");
            Assert.AreEqual(SolvePuzzle1("389125467", 5), "36792584");
            Assert.AreEqual(SolvePuzzle1("389125467", 6), "93672584");
            Assert.AreEqual(SolvePuzzle1("389125467", 7), "92583674");
            Assert.AreEqual(SolvePuzzle1("389125467", 10), "92658374");
            Assert.AreEqual(SolvePuzzle1("389125467", 100), "67384529");
        }

        [TestMethod]
        public void Puzzle1()
        {
            var result = SolvePuzzle1("362981754", 100);
            Assert.AreEqual("24798635", result);
        }

        #endregion

        #region Puzzle 2

        private long SolvePuzzle2(string input, int iterations, int numbers)
        {
            Console.WriteLine($"Input: {input}. Iterations: {iterations}");

            var cups = CupGame.Parse(input, numbers);
            var selected = cups.Start!;

            for (var i = 1; i <= iterations; i++)
            {
                //Console.WriteLine($"{i}:{cups.GetValuesAfter(selected, 9).Aggregate("", (p, n) => $"{p} {n}")}");
                var nextThree = cups.PickupThree(selected);
                //                Console.WriteLine($"Selected {selected.Value}, Pick up {string.Join("", nextThree.Select(x => x.Value))}");
                //Console.WriteLine($"{i}:{cups.GetValuesAfter(selected, 9).Aggregate("", (p, n) => $"{p} {n}")}");
                cups.PutDownThree(nextThree, selected);
                //Console.WriteLine($"{i}:{cups.GetValuesAfter(selected, 9).Aggregate("", (p, n) => $"{p} {n}")}");
                selected = cups.GetNext(selected);
            }

            Console.WriteLine($"Final:{cups.GetValuesAfter(selected, 10).Aggregate("", (p, n) => $"{p} {n}")}");
            var result = cups.GetValuesAfter(cups.Cups[1], 2);
            Console.WriteLine($"{result[0]} * {result[1]}");
            return (long)result[0] * result[1];
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        public void Setup2_1(int iterations)
        {
            SolvePuzzle2("389125467", iterations, 100_000);

            Timed.PrintResults();
        }

        [DataTestMethod]
        [DataRow(1000, 100)]
        [DataRow(10000, 1000)]
        [DataRow(100_000, 10_000)]
        //[DataRow(500_000, 50_000)]
        [DataRow(1_000_000, 100_000)]
        //[DataRow(10_000_000, 1_000_000)]
        //[DataRow(100_000)]
        public void Setup2_PERF(int iterations, int number)
        {
            SolvePuzzle2("389125467", iterations, number);

            Timed.PrintResults();
        }

        [TestMethod]
        public void Setup2()
        {
            Assert.AreEqual(SolvePuzzle2("389125467", 10_000_000, 1_000_000), 149245887792);
        }

        [TestMethod]
        public void Puzzle2()
        {
            Assert.AreEqual(SolvePuzzle2("362981754", 10_000_000, 1_000_000), 12757828710);
        }

        #endregion
    }
}