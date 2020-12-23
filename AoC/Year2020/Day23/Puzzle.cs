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

            var cups = Cups.Parse(input);
            var selected = cups.Order[0];

            for (var i = 1; i <= iterations; i++)
            {
                Console.WriteLine($"{i}:{cups.Order.Aggregate("", (p, n) => $"{p}{n}")}");

                var nextThree = cups.PickupThree(selected);
                Console.WriteLine($"Selected, {i}, Pick up {string.Join("", nextThree)}");
                cups.PutDownThree(nextThree, selected);
                selected = cups.GetNext(selected);
            }

            Console.WriteLine($"Final:{cups.Order.Aggregate("", (p, n) => $"{p}{n}")}");
            return cups.GetValuesAfter(1, cups.Length - 1).Aggregate("", (p, n) => $"{p}{n}");
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

            var cups = Cups.Parse(input, numbers);
            var selected = cups.Order[0];

            using (new Timed("Game"))
            {
                for (var i = 1; i <= iterations; i++)
                {
                    //Console.WriteLine($"{i}:{cups.Order.Aggregate("", (p, n) => $"{p}{n}")}");

                    var nextThree = cups.PickupThree(selected);
                    //Console.WriteLine($"Selected: {selected}, Pick up {string.Join(" ", nextThree)}");
                    cups.PutDownThree(nextThree, selected);
                    selected = cups.GetNext(selected);
                }
            }

            Console.WriteLine($"Last selected {selected}");
            Console.WriteLine($"Final:{cups.Order.Take(11).Aggregate("", (p, n) => $"{p} {n}")}");
            Console.WriteLine($"Last: {cups.Order.Skip(cups.Length - 11).Aggregate("", (p, n) => $"{p} {n}")}");
            var result = cups.GetValuesAfter(1, 2);
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

        //[TestMethod]
        public void Puzzle2()
        {
            Assert.AreEqual(SolvePuzzle2("362981754", 10_000_000, 1_000_000), 149245887792);
        }

        #endregion
    }
}