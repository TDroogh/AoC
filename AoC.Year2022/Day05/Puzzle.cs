using System.Text;
using Xunit.Abstractions;

namespace AoC.Year2022.Day05
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
            public const string Setup1 = "CMZ";
            public const string Puzzle1 = "ZBDRNPMVH";
            public const string Setup2 = "MCD";
            public const string Puzzle2 = "WDLPFNNNB";
        }

        public class Rearrangement
        {
            public string From { get; set; }
            public string To { get; set; }
            public int Amount { get; set; }
        }

        private class Problem
        {
            public Dictionary<string, Stack<char>> Stacks { get; }
            public List<Rearrangement> Rearrangements { get; }

            public Problem()
            {
                Stacks = new Dictionary<string, Stack<char>>();
                Rearrangements = new List<Rearrangement>();
            }

            public void Rearrange(Rearrangement rearrangement)
            {
                for (var i = 0; i < rearrangement.Amount; i++)
                {
                    var pop = Stacks[rearrangement.From].Pop();
                    Stacks[rearrangement.To].Push(pop);
                }
            }

            public void Rearrange2(Rearrangement rearrangement)
            {
                var toPop = new Stack<char>();
                for (var i = 0; i < rearrangement.Amount; i++)
                {
                    var pop = Stacks[rearrangement.From].Pop();
                    toPop.Push(pop);
                }

                foreach (var pop in toPop)
                {
                    Stacks[rearrangement.To].Push(pop);
                }
            }

            public string GetUpperCrates()
            {
                var sb = new StringBuilder();
                foreach (var stack in Stacks.Values)
                {
                    sb.Append(stack.Peek());
                }

                return sb.ToString();
            }

            public static Problem Parse(string[] lines)
            {
                var problem = new Problem();
                var emptyIndex = lines.ToList().IndexOf("");
                var indexers = lines[emptyIndex - 1];

                for (var i = 1; i < indexers.Length; i += 4)
                {
                    if (char.IsDigit(indexers[i]))
                    {
                        var stack = new Stack<char>();
                        var indexer = indexers[i].ToString();

                        for (var j = emptyIndex - 2; j >= 0; j--)
                        {
                            var crate = lines[j][i];
                            if (char.IsUpper(crate))
                            {
                                stack.Push(crate);
                            }
                        }

                        problem.Stacks.Add(indexer, stack);
                    }
                }

                for (var i = emptyIndex + 1; i < lines.Length; i++)
                {
                    var split = lines[i].Split(' ');
                    var rearrangement = new Rearrangement
                    {
                        Amount = int.Parse(split[1]),
                        From = split[3],
                        To = split[5]
                    };

                    problem.Rearrangements.Add(rearrangement);
                }

                return problem;
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var problem = Problem.Parse(input);

            foreach (var rearrangement in problem.Rearrangements)
            {
                _helper.WriteLine($"Rearrangement {rearrangement.Amount} from {rearrangement.From} to {rearrangement.To}");
                foreach (var stack in problem.Stacks)
                {
                    _helper.WriteLine($"{stack.Key}: {new string(stack.Value.ToArray())}");
                }

                problem.Rearrange(rearrangement);
            }

            return problem.GetUpperCrates();
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
            var problem = Problem.Parse(input);

            foreach (var rearrangement in problem.Rearrangements)
            {
                _helper.WriteLine($"Rearrangement {rearrangement.Amount} from {rearrangement.From} to {rearrangement.To}");
                foreach (var stack in problem.Stacks)
                {
                    _helper.WriteLine($"{stack.Key}: {new string(stack.Value.ToArray())}");
                }

                problem.Rearrange2(rearrangement);
            }

            return problem.GetUpperCrates();
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
