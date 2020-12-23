using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020.Day22
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private class Player
        {
            public int Id { get; set; }
            public Queue<int> Cards { get; set; }
        }

        private List<Player> ParsePlayers(string[] input)
        {
            Player current = null;
            List<Player> players = new List<Player>();
            foreach (var line in input)
            {
                if (line.StartsWith("Player"))
                {
                    Assert.IsNull(current);
                    current = new Player
                    {
                        Id = int.Parse(line.Split(new[] { "Player ", ":" }, StringSplitOptions.RemoveEmptyEntries)[0]),
                        Cards = new Queue<int>()
                    };
                }
                else if (string.IsNullOrWhiteSpace(line))
                {
                    Assert.IsNotNull(current);
                    players.Add(current);
                    current = null;
                }
                else
                {
                    Assert.IsNotNull(current);
                    current.Cards.Enqueue(int.Parse(line));
                }
            }

            if (current != null)
                players.Add(current);

            return players;
        }

        private object SolvePuzzle1(string[] input)
        {
            var players = ParsePlayers(input);

            var winner = PlayCombat(players);

            return GetWinnerScore(players, winner);
        }

        private static int PlayCombat(List<Player> players)
        {
            var i = 0;
            const int max = 5000;

            var player1 = players[0];
            var player2 = players[1];

            var previous1 = new List<string>();
            var previous2 = new List<string>();

            while (players.All(x => x.Cards.Any()) && i++ < max)
            {
                Console.WriteLine();
                Console.WriteLine($"Game 2 Round {i}: ");
                //Infinite loop detection
                var cards1 = string.Join(",", player1.Cards);
                Console.WriteLine($"Game 2 Round {i} Player 1: {cards1}");
                var cards2 = string.Join(",", player2.Cards);
                Console.WriteLine($"Game 2 Round {i} Player 2: {cards2}");

                if (previous1.Contains(cards1))
                    return 1;
                if (previous2.Contains(cards2))
                    return 1;

                previous1.Add(cards1);
                previous2.Add(cards2);

                var card0 = player1.Cards.Dequeue();
                var card1 = player2.Cards.Dequeue();

                var firstWins = card0 > card1;
                Console.WriteLine($"Game 1 Round {i}: {card0} vs {card1}.");

                if (firstWins)
                {
                    Console.WriteLine($"Game 1 Round {i}: Winner Player 1!");
                    player1.Cards.Enqueue(card0);
                    player1.Cards.Enqueue(card1);
                }
                else
                {
                    Console.WriteLine($"Game 1 Round {i}: Winner Player 2!");
                    player2.Cards.Enqueue(card1);
                    player2.Cards.Enqueue(card0);
                }
            }

            Assert.IsTrue(i < max);

            return players.Single(x => x.Cards.Any()).Id;
        }

        private static object GetWinnerScore(List<Player> players, int? winnerId = null)
        {
            var winner = winnerId.HasValue
                ? players.Single(x => x.Id == winnerId)
                : players.Single(x => x.Cards.Any());

            var value = winner.Cards.Count;
            var score = 0;
            while (winner.Cards.Any())
            {
                var next = winner.Cards.Dequeue();
                score += value * next;
                value--;
            }

            return score;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(306, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(31314, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input)
        {
            var players = ParsePlayers(input);
            var i = 0;

            var player1 = players[0];
            var player2 = players[1];

            var previous1 = new List<string>();
            var previous2 = new List<string>();

            while (players.All(x => x.Cards.Any()) && i++ < 1000)
            {
                Console.WriteLine();
                Console.WriteLine($"Game 2 Round {i}: ");
                //Infinite loop detection
                var cards1 = string.Join(",", player1.Cards);
                Console.WriteLine($"Game 2 Round {i} Player 1: {cards1}");
                var cards2 = string.Join(",", player2.Cards);
                Console.WriteLine($"Game 2 Round {i} Player 2: {cards2}");

                if (previous1.Contains(cards1))
                    return GetWinnerScore(players, 1);
                if (previous2.Contains(cards2))
                    return GetWinnerScore(players, 1);

                previous1.Add(cards1);
                previous2.Add(cards2);

                var card1 = player1.Cards.Dequeue();
                var card2 = player2.Cards.Dequeue();

                bool player1Wins;
                if (players[0].Cards.Count >= card1 && players[1].Cards.Count >= card2)
                {
                    Console.WriteLine($"Game 2 Round {i}: {card1} vs {card2}. MINIGAME! --");
                    var newPlayers = new List<Player>
                    {
                        new Player{Id = player1.Id, Cards = new Queue<int>(player1.Cards.Take(card1))},
                        new Player{Id = player2.Id, Cards = new Queue<int>(player2.Cards.Take(card2))},
                    };

                    PlayCombat(newPlayers);

                    player1Wins = PlayCombat(newPlayers) == 1;

                    Console.WriteLine($"Game 2 Round {i}: {card1} vs {card2}. MINIGAME Winner: {(player1Wins ? "Player 1!" : "Player 2!")}");
                }
                else
                {
                    player1Wins = card1 > card2;

                    Console.WriteLine($"Game 2 Round {i}: {card1} vs {card2}. Winner: {(player1Wins ? "Player 1!" : "Player 2!")}");
                }

                if (player1Wins)
                {
                    player1.Cards.Enqueue(card1);
                    player1.Cards.Enqueue(card2);
                }
                else
                {
                    player2.Cards.Enqueue(card2);
                    player2.Cards.Enqueue(card1);
                }
            }

            Assert.IsTrue(i < 1000);

            return GetWinnerScore(players);
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(291, result);
        }

        [TestMethod]
        public void Setup2B()
        {
            var input = InputReader.ReadInput("setup2");
            var result = SolvePuzzle2(input);
            Assert.AreEqual(105, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(32760, result);
        }

        #endregion
    }
}