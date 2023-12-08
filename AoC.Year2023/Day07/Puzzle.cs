namespace AoC.Year2023.Day07
{
    public class Puzzle(ITestOutputHelper output)
    {
        private static class Results
        {
            public const int Setup1 = 6440;
            public const int Puzzle1 = 248105065;
            public const int Setup2 = 5905;
            public const int Puzzle2 = 249515436;
        }

        public class Hand : IComparable<Hand>
        {
            public required Card[] Cards { get; init; }
            public required string CardString { get; init; }
            public int Bid { get; init; }
            public Score Score { get; init; }
            public Score Score2 { get; init; }
            public int CardValues { get; init; }

            public static Hand Parse(string input)
            {
                var split = input.Split(" ");
                var cards = split[0].Select(GetCard).ToArray();

                return new Hand
                {
                    Cards = split[0].Select(GetCard).ToArray(),
                    CardString = split[0],
                    Bid = int.Parse(split[1]),
                    Score = GetScore(split[0]),
                    Score2 = GetScore2(split[0]),
                    CardValues = GetCardValue2(cards)
                };
            }

            private static Card GetCard(char chr)
            {
                return chr switch
                {
                    '2' => Card.Two,
                    '3' => Card.Three,
                    '4' => Card.Four,
                    '5' => Card.Five,
                    '6' => Card.Six,
                    '7' => Card.Seven,
                    '8' => Card.Eight,
                    '9' => Card.Nine,
                    'T' => Card.Ten,
                    'J' => Card.Jack,
                    'Q' => Card.Queen,
                    'K' => Card.King,
                    'A' => Card.Ace,
                    _ => throw new InvalidOperationException()
                };
            }

            private static Score GetScore2(string cards)
            {
                var distinct = cards.Distinct().ToArray();
                var bestScore = Score.HighCard;

                foreach (var replace in distinct)
                {
                    var replaced = cards.Replace('J', replace);
                    var score = GetScore(replaced);

                    if (score > bestScore)
                    {
                        bestScore = score;
                    }
                }

                return bestScore;
            }

            public static int GetCardValue2(Card[] cards)
            {
                int GetValue(Card card)
                {
                    if (card == Card.Jack)
                    {
                        return 0;
                    }
                    return (int)card;
                }

                return GetValue(cards[0]) * 100000000
                       + GetValue(cards[1]) * 1000000
                       + GetValue(cards[2]) * 10000
                       + GetValue(cards[3]) * 100
                       + GetValue(cards[4]) * 1;
            }

            private static Score GetScore(string cards)
            {
                var distinct = cards.Distinct().ToArray();

                return distinct.Length switch
                {
                    5 => Score.HighCard,
                    4 => Score.OnePair,
                    3 => distinct.Max(d => cards.Count(c => c == d)) == 3 ? Score.ThreeOfAKind : Score.TwoPair,
                    2 => distinct.Max(d => cards.Count(c => c == d)) == 3 ? Score.FullHouse : Score.FourOfAKind,
                    _ => Score.FiveOfAKind,
                };
            }

            public int CompareTo(Hand? other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;

                var compare = Score.CompareTo(other.Score);
                if (compare != 0)
                {
                    return compare;
                }

                return CompareHands(Cards, other.Cards);
            }

            public static int CompareHands(Card[] left, Card[] right)
            {
                for (var i = 0; i < left.Length; i++)
                {
                    var c = left[i];
                    var o = right[i];

                    if (c < o)
                    {
                        return -1;
                    }
                    if (c > o)
                    {
                        return 1;
                    }
                }

                return 0;
            }
        }

        public enum Score
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind
        }

        public enum Card
        {
            Two = 1,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var hands = input.Select(Hand.Parse).ToList();
            var ordered = hands.Order().ToList();

            var sum = 0;
            for (var i = 1; i <= ordered.Count; i++)
            {
                var hand = ordered[i - 1];
                sum += hand.Bid * i;

                output.WriteLine($"{i}: {hand.CardString} - {hand.Bid * i}");
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
            var hands = input.Select(Hand.Parse).ToList();
            var ordered = hands.OrderBy(val => val.Score2).ThenBy(val => val.CardValues).ToList();

            var sum = 0;
            for (var i = 1; i <= ordered.Count; i++)
            {
                var hand = ordered[i - 1];
                sum += hand.Bid * i;

                output.WriteLine($"{i:D4}: {hand.CardString} - {hand.Bid:D4} - {hand.CardValues:D9}");
            }

            return sum;
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
