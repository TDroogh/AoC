using Xunit.Abstractions;

namespace AoC.Year2023.Day08
{
    public class Puzzle(ITestOutputHelper helper)
    {
        private static class Results
        {
            public const int Setup1 = 6;
            public const int Puzzle1 = 13019;
            public const long Setup2 = 6;
            public const long Puzzle2 = 13524038372771;
        }

        public class Network
        {
            public required char[] Route { get; set; }
            public required Dictionary<string, Node> Nodes { get; set; }

            public IEnumerable<char> GetRoute()
            {
                while (true)
                {
                    foreach (var turn in Route)
                    {
                        yield return turn;
                    }
                }
                // ReSharper disable once IteratorNeverReturns
            }

            public static Network Parse(string[] input)
            {
                var network = new Network
                {
                    Route = input[0].ToCharArray(),
                    Nodes = input.Skip(2).Select(Node.Parse).ToDictionary(x => x.Identifier)
                };

                foreach (var (_, value) in network.Nodes)
                {
                    value.LeftNode = network.Nodes[value.Left];
                    value.RightNode = network.Nodes[value.Right];
                }

                return network;
            }
        }

        public class Node
        {
            public required string Identifier { get; set; }
            public required string Left { get; set; }
            public Node? LeftNode { get; set; }
            public required string Right { get; set; }
            public Node? RightNode { get; set; }
            public bool IsEnd { get; set; }

            public static Node Parse(string input)
            {
                var split = input.Split(new[] { ' ', '=', '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);

                return new Node
                {
                    Identifier = split[0],
                    Left = split[1],
                    Right = split[2],
                    IsEnd = split[0].EndsWith('Z')
                };
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var network = Network.Parse(input);
            var currentNode = network.Nodes["AAA"];

            return GetNumberOfSteps(network, currentNode, false);
        }

        private static int GetNumberOfSteps(Network network, Node currentNode, bool endsWithZ)
        {
            var steps = 0;
            foreach (var step in network.GetRoute())
            {
                steps++;
                currentNode = (step == 'L' ? currentNode.LeftNode : currentNode.RightNode) ?? throw new ArgumentNullException();

                if (endsWithZ ? currentNode.IsEnd : currentNode.Identifier == "ZZZ")
                {
                    break;
                }
            }

            return steps;
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
            var network = Network.Parse(input);
            long result = 1;
            foreach (var node in network.Nodes.Values.Where(x => x.Identifier.EndsWith('A')))
            {
                var numberOfSteps = GetNumberOfSteps(network, node, true);

                helper.WriteLine($"{node.Identifier}: {numberOfSteps} steps, result {result}");

                result = MathHelper.LeastCommonMultiple(result, numberOfSteps);
            }

            return result;
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadInput("setup2");
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
