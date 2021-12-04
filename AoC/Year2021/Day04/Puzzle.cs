using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day04
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 4512;
            public const int Puzzle1 = 63552;
            public const int Setup2 = 1924;
            public const int Puzzle2 = 9020;
        }

        private class BingoCard
        {
            private const int CardSize = 5;

            private BingoCard()
            {
                Numbers = new int[CardSize,CardSize];
                Marked = new bool[CardSize,CardSize];
            }

            private int[,] Numbers { get; }
            private bool[,] Marked { get; }

            public void AddNumber(int number)
            {
                for (var i = 0; i < CardSize; i++)
                {
                    for (var j = 0; j < CardSize; j++)
                    {
                        if (Numbers[i, j] == number)
                        {
                            Marked[i, j] = true;
                        }
                    }
                }
            }

            public bool HasMatch()
            {
                for (var i = 0; i < CardSize; i++)
                {
                    if (Marked[i, 0] && Marked[i, 1] && Marked[i, 2] && Marked[i, 3] && Marked[i, 4])
                        return true;

                    if (Marked[0, i] && Marked[1, i] && Marked[2, i] && Marked[3, i] && Marked[4, i])
                        return true;
                }

                return false;
            }

            public int GetUnmarkedSum()
            {
                var sum = 0;

                for (var i = 0; i < CardSize; i++)
                {
                    for (var j = 0; j < CardSize; j++)
                    {
                        if (Marked[i, j] == false)
                            sum += Numbers[i, j];
                    }
                }

                return sum;
            }

            public static BingoCard Parse(string[] lines)
            {
                Assert.IsTrue(lines.Length == CardSize);

                var card = new BingoCard();
                for (var r = 0; r < CardSize; r++)
                {
                    var row = lines[r];
                    var values = row.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    Assert.IsTrue(values.Length == CardSize);
                    for(var c = 0; c < CardSize; c++)
                    {
                        card.Numbers[r, c] = values[c];
                    }
                }

                return card;
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var drawnNumbers = input[0].Split(",").Select(int.Parse).ToList();
            var cards = ParseCards(input).ToList();

            foreach (var number in drawnNumbers)
            {
                foreach (var card in cards)
                {
                    card.AddNumber(number);
                    if (card.HasMatch())
                    {
                        return number * card.GetUnmarkedSum();
                    }
                }
            }

            return -1;
        }

        private static IEnumerable<BingoCard> ParseCards(string[] input)
        {
            for (var i = 2; i < input.Length; i += 6)
            {
                var cardInput = input.Skip(i).Take(5).ToArray();
                yield return BingoCard.Parse(cardInput);
            }
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Setup1, result);
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
            var drawnNumbers = input[0].Split(",").Select(int.Parse).ToList();
            var cards = ParseCards(input).ToList();

            foreach (var number in drawnNumbers)
            {
                foreach (var card in cards.ToArray())
                {
                    card.AddNumber(number);
                    if (card.HasMatch())
                    {
                        if (cards.Count == 1)
                            return number * card.GetUnmarkedSum();
                        
                        cards.Remove(card);
                    }
                }
            }

            return -1;
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
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