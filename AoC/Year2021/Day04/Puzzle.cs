using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

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
            public const int CardSize = 5;

            private BingoCard()
            {
                _numbers = new int[CardSize, CardSize];
                _marked = new bool[CardSize, CardSize];
            }

            private readonly int[,] _numbers;
            private readonly bool[,] _marked;

            public void AddNumber(int number)
            {
                for (var i = 0; i < CardSize; i++)
                {
                    for (var j = 0; j < CardSize; j++)
                    {
                        if (_numbers[i, j] == number)
                        {
                            _marked[i, j] = true;
                        }
                    }
                }
            }

            public bool HasMatch()
            {
                for (var i = 0; i < CardSize; i++)
                {
                    if (_marked[i, 0] && _marked[i, 1] && _marked[i, 2] && _marked[i, 3] && _marked[i, 4])
                        return true;

                    if (_marked[0, i] && _marked[1, i] && _marked[2, i] && _marked[3, i] && _marked[4, i])
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
                        if (_marked[i, j] == false)
                            sum += _numbers[i, j];
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
                    for (var c = 0; c < CardSize; c++)
                    {
                        card._numbers[r, c] = values[c];
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