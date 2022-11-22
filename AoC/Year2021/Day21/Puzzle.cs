using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2021.Day21
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 739785;
            public const int Puzzle1 = 752247;
            public const long Setup2 = 444_356_092_776_315;
            public const long Puzzle2 = 221_109_915_584_112;
        }

        public class DeterministicDice
        {
            public DeterministicDice(int size)
            {
                _size = size;
            }

            private readonly int _size;
            public int Counter { get; private set; }

            public int GetNextValue()
            {
                return Counter++ % _size + 1;
            }
        }

        public class Player
        {
            public int Number { get; set; }
            public int CurrentNumber { get; set; }
            public int CurrentPoints { get; set; }

            public static Player Parse(string input)
            {
                var split = input.Replace("Player ", "").Split(" starting position: ");

                Console.WriteLine($"Player {split[0]} starts at position {split[1]}.");

                return new Player
                {
                    Number = int.Parse(split[0]),
                    CurrentPoints = 0,
                    CurrentNumber = int.Parse(split[1])
                };
            }

            public static IEnumerable<Player> ParseMany(string[] input)
            {
                foreach (var line in input)
                    yield return Parse(line);
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var players = Player.ParseMany(input).ToList();
            var die = new DeterministicDice(10);

            while (players.Any(x => x.CurrentPoints >= 1000) == false)
            {
                var turn = die.Counter % 6;
                var value = die.GetNextValue();
                var player = players[turn <= 2 ? 0 : 1];

                player.CurrentNumber += value;
                if (turn is 2 or 5)
                    player.CurrentPoints += (player.CurrentNumber - 1) % 10 + 1;

                Console.WriteLine($"Turn {die.Counter}: Player {player.Number} has {player.CurrentPoints} points and is at position {player.CurrentNumber % 10}");
            }

            foreach (var player in players)
                Console.WriteLine($"Player {player.Number} has {player.CurrentPoints} points and is at position {player.CurrentNumber % 10}");

            return die.Counter * players.Min(x => x.CurrentPoints);
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

        public class Universe
        {
            public long Count { get; set; }

            public Player Player1 { get; set; }
            public Player Player2 { get; set; }

            public Universe Copy(int count)
            {
                return new Universe
                {
                    Count = Count * count,
                    Player1 = new Player
                    {
                        CurrentNumber = Player1.CurrentNumber,
                        CurrentPoints = Player1.CurrentPoints,
                    },
                    Player2 = new Player
                    {
                        CurrentNumber = Player2.CurrentNumber,
                        CurrentPoints = Player2.CurrentPoints,
                    }
                };
            }

            public bool Equals(Universe other)
            {
                return Player1.CurrentPoints == other.Player1.CurrentPoints
                       || Player1.CurrentNumber == other.Player1.CurrentNumber
                       || Player2.CurrentPoints == other.Player2.CurrentPoints
                       || Player2.CurrentNumber == other.Player2.CurrentNumber;
            }

            public int CalculateKey()
            {
                return Player1.CurrentNumber - 1
                       + (Player2.CurrentNumber - 1) * 10
                       + (Player1.CurrentPoints) * 1000
                       + (Player2.CurrentPoints) * 100000;
            }
        }

        private object SolvePuzzle2(string[] input)
        {
            var players = Player.ParseMany(input).ToList();
            var universes = new List<Universe>
            {
                new Universe
                {
                    Count = 1,
                    Player1 = players[0],
                    Player2 = players[1]
                }
            };

            var options = new Dictionary<int, int>
            {
                { 3, 1 },
                { 4, 3 },
                { 5, 6 },
                { 6, 7 },
                { 7, 6 },
                { 8, 3 },
                { 9, 1 },
            };

            var i = 0;
            long p1Wins = 0;
            long p2Wins = 0;
            while (i < 20)
            {
                var player1Turn = (i++ % 2) == 0;
                var newUniverses = new Dictionary<int, Universe>();

                foreach (var universe in universes)
                {
                    foreach (var (sum, count) in options)
                    {
                        var newUniverse = universe.Copy(count);
                        var playerTurn = player1Turn ? newUniverse.Player1 : newUniverse.Player2;
                        var newNumber = (playerTurn.CurrentNumber + sum - 1) % 10 + 1;

                        playerTurn.CurrentNumber = newNumber;
                        playerTurn.CurrentPoints += newNumber;

                        //If a player wins add the number of universes to the total
                        if (playerTurn.CurrentPoints >= 21)
                        {
                            if (player1Turn)
                                p1Wins += newUniverse.Count;
                            else
                                p2Wins += newUniverse.Count;
                        }
                        //If it is not decided yet, add the new universes to the new collection
                        //Take into account that it is possible that we have duplicate universes
                        else
                        {
                            var key = newUniverse.CalculateKey();
                            if (newUniverses.TryGetValue(key, out var value))
                                value.Count += newUniverse.Count;
                            else
                                newUniverses[key] = newUniverse;
                        }
                    }
                }

                Console.WriteLine($"Iteration {i}: {p1Wins} for player 1, {p2Wins} for player 2");
                universes = newUniverses.Values.ToList();
                Console.WriteLine($"Iteration {i}: {universes.Count} distinct universes left");
            }

            return Math.Max(p1Wins, p2Wins);
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