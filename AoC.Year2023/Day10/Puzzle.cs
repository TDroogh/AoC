namespace AoC.Year2023.Day10
{
    public class Puzzle(ITestOutputHelper helper)
    {
        private static class Results
        {
            public const int Setup1 = 11;
            public const int Puzzle1 = 6846;
            public const int Setup2 = 10;
            public const int Puzzle2 = 325;
        }

        private enum Direction
        {
            Down = 1,
            Left = 2,
            Right = 3,
            Up = 4
        }

        #region Puzzle 1

        private object SolvePuzzle1(char[,] input)
        {
            var results = GetResults(input);

            results.Print(true, helper.WriteLine, 4);

            return results.GetAllValues().Max();
        }

        private int[,] GetResults(char[,] input, bool biDirectional = true)
        {
            var startingPoint = input.GetAllPoints().Single(point => input[point.x, point.y] == 'S');
            var results = new int[input.GetLength(0), input.GetLength(1)];
            foreach (var (x, y) in results.GetAllPoints())
            {
                results[x, y] = -1;
            }
            results[startingPoint.x, startingPoint.y] = 0;

            var round = 0;
            do
            {
                var newRoutesFound = 0;
                foreach (var (x2, y2) in results.GetAllPoints().Where(p => results[p.x, p.y] == round))
                {
                    foreach (var (x, y) in results.GetAdjacentPoints(x2, y2, false))
                    {
                        if (results[x, y] != -1)
                        {
                            continue;
                        }

                        if (IsMovePossible(input, x, x2, y, y2))
                        {
                            results[x, y] = round + 1;
                            newRoutesFound++;

                            if (!biDirectional)
                            {
                                break;
                            }
                        }
                    }
                }

                if (newRoutesFound == 0)
                {
                    break;
                }

                round++;
            } while (round < 100_000);

            return results;
        }

        private static bool IsMovePossible(char[,] input, int x, int x2, int y, int y2)
        {
            var from = input[x2, y2];
            var to = input[x, y];
            var direction = GetDirection(x, x2, y, y2);

            return direction switch
            {
                Direction.Down => CanGoDown(from) && CanGoUp(to),
                Direction.Up => CanGoUp(from) && CanGoDown(to),
                Direction.Left => CanGoLeft(from) && CanGoRight(to),
                Direction.Right => CanGoRight(from) && CanGoLeft(to),
                _ => throw new InvalidOperationException()
            };
        }

        private static bool CanGoUp(char chr)
        {
            return chr is 'S' or '|' or 'J' or 'L';
        }

        private static bool CanGoDown(char chr)
        {
            return chr is 'S' or '|' or '7' or 'F';
        }

        private static bool CanGoLeft(char chr)
        {
            return chr is 'S' or '-' or 'J' or '7';
        }

        private static bool CanGoRight(char chr)
        {
            return chr is 'S' or '-' or 'L' or 'F';
        }

        private static Direction GetDirection(int x, int x2, int y, int y2)
        {
            if (x > x2)
            {
                return Direction.Right;
            }
            if (x2 > x)
            {
                return Direction.Left;
            }

            return y > y2 ? Direction.Down : Direction.Up;
        }

        [Fact]
        public void Setup1()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(Results.Setup1, result);
        }

        [Fact]
        public void Puzzle1()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(char[,] input)
        {
            var results = GetResults(input, false);
            input.Print(true, helper.WriteLine);

            var count = 0;
            foreach (var (x, y) in results.GetAllPoints())
            {
                if (IsEnclosed(input, results, x, y))
                {
                    results[x, y] = 0;
                    count++;
                }
            }
            results.Print(true, helper.WriteLine);
            return count;
        }

        private bool IsEnclosed(char[,] input, int[,] results, int x, int y)
        {
            var value = results[x, y];
            if (value != -1)
            {
                return false;
            }

            var leftCount = 0;
            var prevDirection = Direction.Left;
            for (var x2 = 0; x2 < x; x2++)
            {
                var val = results[x2, y];
                if (val == -1)
                {
                    continue;
                }

                var pipe = input[x2, y];
                if (pipe is '|' or 'F' or '7')
                {
                    var other = results[x2, y + 1];
                    var direction = other > val ? Direction.Down : Direction.Up;
                    if (direction != prevDirection && other > -1)
                    {
                        leftCount++;
                        prevDirection = direction;
                    }
                }
                if (pipe is 'J' or 'L')
                {
                    var other = results[x2, y - 1];
                    var direction = other < val ? Direction.Down : Direction.Up;
                    if (direction != prevDirection && other > -1)
                    {
                        leftCount++;
                        prevDirection = direction;
                    }
                }
            }

            var rightCount = 0;
            prevDirection = Direction.Left;
            for (var x2 = x; x2 < results.GetLength(0); x2++)
            {
                var val = results[x2, y];
                if (val == -1)
                {
                    continue;
                }

                var pipe = input[x2, y];
                if (pipe is '|' or 'F' or '7')
                {
                    var other = results[x2, y + 1];
                    var direction = other > val ? Direction.Down : Direction.Up;
                    if (direction != prevDirection && other > -1)
                    {
                        rightCount++;
                        prevDirection = direction;
                    }
                }
                if (pipe is 'J' or 'L')
                {
                    var other = results[x2, y - 1];
                    var direction = other < val ? Direction.Down : Direction.Up;
                    if (direction != prevDirection && other > -1)
                    {
                        rightCount++;
                        prevDirection = direction;
                    }
                }
            }

            var res = leftCount > 0 && rightCount > 0 && (leftCount % 2 != 0 || rightCount % 2 != 0);

            helper.WriteLine($"({x}, {y}): left: {leftCount}, right: {rightCount}, result: {(res ? "Yes" : "No")}");

            return res;
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadArrayInput(1, "setup-2");
            var result = SolvePuzzle2(input);
            Assert.Equal(Results.Setup2, result);

            var inputB = InputReader.ReadArrayInput(1, "setup-2b");
            var resultB = SolvePuzzle2(inputB);
            Assert.Equal(8, resultB);
        }

        [Fact]
        public void Puzzle2()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle2(input);
            Assert.Equal(Results.Puzzle2, result);
        }

        #endregion
    }
}
