using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020.Day23
{
    [TestClass]
    public class Puzzle
    {
        public class CupGame
        {
            private CupGame(int count)
            {
                Cups = new Dictionary<int, Cup>();
            }

            public Cup Start { get; set; }
            public Dictionary<int, Cup> Cups { get; }

            public static CupGame Parse(string input, int fill = 9)
            {
                var game = new CupGame(fill);

                var previous = (Cup) null;
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
                    previous.Next = game.Start;

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
                    result[i] = previous.Next;
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

                return after.Next;
            }
        }

        public class Cup
        {
            public int Value { get; set; }
            public Cup Next { get; set; }

            /// <inheritdoc />
            public override string ToString()
            {
                return $"Cup {Value}, Next {Next?.Value}";
            }
        }

        #region Puzzle 1

        public class Cups
        {
            private Cups(int count)
            {
                Order = new List<int>();
            }

            public int Length { get; set; }
            public List<int> Order { get; }

            public static Cups Parse(string input, int fill = 9)
            {
                var cups = new Cups(fill);

                for (var i = 1; i <= fill; i++)
                {
                    if (i - 1 < input.Length)
                        cups.Order.Add(int.Parse(input[i - 1].ToString()));
                    else
                        cups.Order.Add(i);
                }

                cups.Length = cups.Order.Count;
                return cups;
            }

            public int[] PickupThree(int after)
            {
                using var _ = new Timed("PickupThree");

                return GetValuesAfter(after, 3);
            }

            public int[] GetValuesAfter(int after, int count)
            {
                using var _ = new Timed("ValuesAfter");

                var result = new int[count];
                var i = Order.IndexOf(after) + 1;

                if (i + count < Length)
                {
                    for (var j = 0; j < count; j++)
                    {
                        result[j] = Order[i + j];
                    }

                    Order.RemoveRange(i, count);
                }
                else
                {
                    for (var j = 0; j < count; j++)
                    {
                        if (i >= Order.Count)
                            i = 0;
                        result[j] = Order[i];
                        Order.RemoveAt(i);
                    }
                }

                return result;
            }

            public void PutDownThree(int[] values, int after)
            {
                using var _ = new Timed("PutDownThree");

                var destination = after - 1;

                var index = Order.IndexOf(destination);
                while (index == -1)
                {
                    if (destination == 0)
                        destination += Length + 1;

                    destination--;

                    index = Order.IndexOf(destination);
                }

                Order.InsertRange(index + 1, values);
            }

            public int GetNext(int after)
            {
                using var _ = new Timed("GetNext");

                var i = Order.IndexOf(after);

                return Order[(i + 1) % Length];
            }
        }

        private class Timed : IDisposable
        {
            private static Dictionary<string, long> _results = new Dictionary<string, long>();

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
                if (_results.ContainsKey(name))
                    _results[name] += sw.ElapsedMilliseconds;
                else
                    _results[name] = sw.ElapsedMilliseconds;
            }

            public static void PrintResults()
            {
                foreach (var kvp in _results)
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }


        private string SolvePuzzle1(string input, int iterations)
        {
            Console.WriteLine($"Input: {input}. Iterations: {iterations}");

            var cups = CupGame.Parse(input);
            var selected = cups.Start;

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
            var selected = cups.Start;

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
            return (long) result[0] * result[1];
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