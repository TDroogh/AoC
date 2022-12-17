using Xunit.Abstractions;

namespace AoC.Year2022.Day17
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
            public const int Setup1 = 3068;
            public const int Puzzle1 = 3186;
            public const int Setup2 = 2;
            public const int Puzzle2 = 2;
        }

        private IEnumerable<char[,]> GetShapes(int count)
        {
            var shape1 = new char[1, 4];
            foreach (var (x, y) in shape1.GetAllPoints())
            {
                shape1[x, y] = '#';
            }

            shape1.Print(false, _helper.WriteLine);

            var shape2 = new char[3, 3];
            foreach (var (x, y) in shape2.GetAllPoints())
            {
                shape2[x, y] = x is 0 or 2 && y is 0 or 2 ? ' ' : '#';
            }

            shape2.Print(false, _helper.WriteLine);

            var shape3 = new char[3, 3];
            foreach (var (x, y) in shape3.GetAllPoints())
            {
                shape3[x, y] = x is 0 or 1 && y is 0 or 1 ? ' ' : '#';
            }

            shape3.Print(false, _helper.WriteLine);

            var shape4 = new char[4, 1];
            foreach (var (x, y) in shape4.GetAllPoints())
            {
                shape4[x, y] = '#';
            }

            shape4.Print(false, _helper.WriteLine);

            var shape5 = new char[2, 2];
            foreach (var (x, y) in shape5.GetAllPoints())
            {
                shape5[x, y] = '#';
            }

            shape5.Print(false, _helper.WriteLine);

            var shapes = new List<char[,]> { shape1, shape2, shape3, shape4, shape5 };

            for (var i = 0; i < count; i++)
            {
                yield return shapes[i % shapes.Count];
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input, int iterations = 2022)
        {
            const int height = 4000;
            var field = new char[height, 9];

            foreach (var (x, y) in field.GetAllPoints())
            {
                var isSide = y is 0 or 8;
                var isBottom = x == field.GetLength(0) - 1;

                field[x, y] = (isSide, isBottom) switch
                {
                    (true, true) => '+',
                    (true, false) => '|',
                    (false, true) => '-',
                    (false, false) => ' '
                };
            }

            var jetPattern = input[0];

            var p = 0;
            var i = 0;
            var highestShape = field.GetLength(0) - 2;

            foreach (var shape in GetShapes(2022))
            {
                var x = highestShape - 3;
                var y = 3;

                PrintWithShape(shape, field, x, y);

                while (true)
                {
                    var movement = jetPattern[p++ % jetPattern.Length];

                    switch (movement)
                    {
                        case '<' when IsValid(shape, field, x, y - 1):
                        {
                            //_helper.WriteLine("Moved left");
                            y--;
                            break;
                        }
                        case '>' when IsValid(shape, field, x, y + 1):
                        {
                            //_helper.WriteLine("Moved right");
                            y++;
                            break;
                        }
                        default:
                        {
                            //_helper.WriteLine($"Cannot move to {(movement == '>' ? "right" : "left")}");
                            break;
                        }
                    }

                    PrintWithShape(shape, field, x, y);

                    //Move down
                    if (IsValid(shape, field, x + 1, y))
                    {
                        //_helper.WriteLine("Moved down");
                        x++;
                    }
                    else
                    {
                        //_helper.WriteLine("Cannot move down");
                        break;
                    }

                    PrintWithShape(shape, field, x, y);
                }

                PlaceShapeInField(shape, field, x, y, false);

                //field.Print(false, _helper.WriteLine);

                highestShape = Math.Min(highestShape, x - shape.GetLength(0));

                _helper.WriteLine($"Iteration {++i} Highest shape: {height - highestShape - 2}");
            }

            field.Print(false, _helper.WriteLine);

            return height - highestShape - 2;
        }

        private static bool IsValid(char[,] shape, char[,] field, int x, int y)
        {
            foreach (var (sx, sy) in shape.GetAllPoints())
            {
                var chr = shape[shape.GetLength(0) - sx - 1, sy];
                if (chr == '#')
                {
                    var fieldChr = field[x - sx, y + sy];
                    if (fieldChr != ' ')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static char Test = '0';
        private static void PlaceShapeInField(char[,] shape, char[,] field, int x, int y, bool isCopy)
        {
            Test++;
            if (Test > '9')
                Test = '0';

            foreach (var (sx, sy) in shape.GetAllPoints())
            {
                var chr = shape[shape.GetLength(0) - sx - 1, sy];
                if (chr == '#')
                {
                    field[x - sx, y + sy] = isCopy ? '@' : Test;
                }
            }
        }

        private void PrintWithShape(char[,] shape, char[,] field, int x, int y)
        {
            //var copy = (char[,])field.Clone();

            //PlaceShapeInField(shape, copy, x, y, true);

            //copy.Print(false, _helper.WriteLine);
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
            return 2;
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
