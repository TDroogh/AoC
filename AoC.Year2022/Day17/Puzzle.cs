using System.Text;
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
            public const long Setup2 = 1514285714288;
            public const long Puzzle2 = 3186;
        }

        private IEnumerable<char[,]> GetShapes(int count)
        {
            var shape1 = new char[1, 4];
            foreach (var (x, y) in shape1.GetAllPoints())
            {
                shape1[x, y] = '#';
            }

            //shape1.Print(false, _helper.WriteLine);

            var shape2 = new char[3, 3];
            foreach (var (x, y) in shape2.GetAllPoints())
            {
                shape2[x, y] = x is 0 or 2 && y is 0 or 2 ? ' ' : '#';
            }

            //shape2.Print(false, _helper.WriteLine);

            var shape3 = new char[3, 3];
            foreach (var (x, y) in shape3.GetAllPoints())
            {
                shape3[x, y] = x is 0 or 1 && y is 0 or 1 ? ' ' : '#';
            }

            //shape3.Print(false, _helper.WriteLine);

            var shape4 = new char[4, 1];
            foreach (var (x, y) in shape4.GetAllPoints())
            {
                shape4[x, y] = '#';
            }

            //shape4.Print(false, _helper.WriteLine);

            var shape5 = new char[2, 2];
            foreach (var (x, y) in shape5.GetAllPoints())
            {
                shape5[x, y] = '#';
            }

            //shape5.Print(false, _helper.WriteLine);

            var shapes = new List<char[,]> { shape1, shape2, shape3, shape4, shape5 };

            for (var i = 0; i < count; i++)
            {
                yield return shapes[i % shapes.Count];
            }
        }

        #region Puzzle 1

        private int SolvePuzzle1(string[] input, int iterations)
        {
            var defaultOffset = 50;
            var defaultHeight = defaultOffset * 2 + 10;
            var totalHeight = defaultHeight;
            var field = InitField1(defaultHeight, null, 0);

            var jetPattern = input[0];
            var offset = 0;
            var p = 0;
            var highestShape = field.GetLength(0) - 2;

            foreach (var shape in GetShapes(iterations))
            {
                var x = highestShape - 3;
                var y = 3;

                while (true)
                {
                    var movement = jetPattern[p++ % jetPattern.Length];

                    switch (movement)
                    {
                        case '<' when IsValid(shape, field, x, y - 1):
                            y--;
                            break;
                        case '>' when IsValid(shape, field, x, y + 1):
                            y++;
                            break;
                    }

                    //Move down
                    if (IsValid(shape, field, x + 1, y))
                    {
                        x++;
                    }
                    else
                    {
                        break;
                    }
                }

                PlaceShapeInField(shape, field, x, y, true);

                highestShape = Math.Min(highestShape, x - shape.GetLength(0));

                if (totalHeight - highestShape - 2 - offset > defaultOffset * 2)
                {
                    offset += defaultOffset;
                    totalHeight += defaultOffset;
                    highestShape += defaultOffset;
                    field = InitField1(defaultHeight, field, defaultOffset);
                }
            }

            var line = new StringBuilder();
            var line2 = new StringBuilder();
            var line3 = new StringBuilder();
            for (var i = 0; i < 9; i++)
            {
                line.Append(field[highestShape + 1, i]);
                line2.Append(field[highestShape + 2, i]);
                line3.Append(field[highestShape + 3, i]);
            }

            _helper.WriteLine(line.ToString());
            _helper.WriteLine(line2.ToString());
            _helper.WriteLine(line3.ToString());

            return totalHeight - highestShape - 2;
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

        private static void PlaceShapeInField(char[,] shape, char[,] field, int x, int y, bool isCopy)
        {
            foreach (var (sx, sy) in shape.GetAllPoints())
            {
                var chr = shape[shape.GetLength(0) - sx - 1, sy];
                if (chr == '#')
                {
                    field[x - sx, y + sy] = isCopy ? '@' : '#';
                }
            }
        }

        //private void PrintWithShape(char[,] shape, char[,] field, int x, int y)
        //{
        //    var copy = (char[,])field.Clone();

        //    PlaceShapeInField(shape, copy, x, y, true);

        //    copy.Print(false, _helper.WriteLine);
        //}

        [Fact]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input, 2022);
            Assert.Equal(Results.Setup1, result);
        }

        [Fact]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input, 2022);
            Assert.Equal(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        public object SolvePuzzle2(string[] input, int iterations = 2022)
        {
            var defaultOffset = 5000;
            var defaultHeight = defaultOffset * 2 + 10;
            var totalHeight = defaultHeight;
            var field = InitField2(defaultHeight, null, 0);

            var jetPattern = input[0];
            var offset = 0;
            var p = 0;
            var highestShape = field.GetLength(0) - 2;

            foreach (var shape in GetShapes(iterations))
            {
                var x = highestShape - 3;
                var y = 3;

                while (true)
                {
                    var movement = jetPattern[p++ % jetPattern.Length];

                    switch (movement)
                    {
                        case '<' when IsValid(shape, field, x, y - 1):
                            y--;
                            break;
                        case '>' when IsValid(shape, field, x, y + 1):
                            y++;
                            break;
                    }

                    //Move down
                    if (IsValid(shape, field, x + 1, y))
                    {
                        x++;
                    }
                    else
                    {
                        break;
                    }
                }

                PlaceShapeInField(shape, field, x, y);

                highestShape = Math.Min(highestShape, x - shape.GetLength(0));

                if (totalHeight - highestShape - 2 - offset > defaultOffset * 2)
                {
                    offset += defaultOffset;
                    totalHeight += defaultOffset;
                    highestShape += defaultOffset;
                    field = InitField2(defaultHeight, field, defaultOffset);
                }
            }

            return totalHeight - highestShape - 2;
        }

        private static char[,] InitField1(int size, char[,]? oldField, int offset)
        {
            var field = new char[size, 9];

            foreach (var (x, y) in field.GetAllPoints())
            {
                var isSide = y is 0 or 8;
                var isBottom = x == field.GetLength(0) - 1;

                if (oldField != null && x - offset > 0)
                {
                    field[x, y] = oldField[x - offset, y];
                }
                else
                {
                    field[x, y] = (isSide, isBottom) switch
                    {
                        (true, true) => '+',
                        (true, false) => '|',
                        (false, true) => '-',
                        (false, false) => ' '
                    };
                }
            }

            return field;
        }

        private static string[] InitField2(int size, string[]? oldField, int offset)
        {
            var field = new string[size];

            for (var x = 0; x < field.Length; x++)
            {
                var isBottom = x == field.GetLength(0) - 1;

                if (oldField != null && x - offset > 0)
                {
                    field[x] = oldField[x - offset];
                }
                else
                {
                    field[x] = isBottom ? "+-------+" : "|       |";
                }
            }

            return field;
        }

        private static bool IsValid(char[,] shape, string[] field, int x, int y)
        {
            foreach (var (sx, sy) in shape.GetAllPoints())
            {
                var chr = shape[shape.GetLength(0) - sx - 1, sy];
                if (chr == '#')
                {
                    var fieldChr = field[x - sx][y + sy];
                    if (fieldChr != ' ')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static void PlaceShapeInField(char[,] shape, string[] field, int x, int y, bool copy = false)
        {
            foreach (var (sx, sy) in shape.GetAllPoints())
            {
                var chr = shape[shape.GetLength(0) - sx - 1, sy];
                if (chr == '#')
                {
                    var line = field[x - sx];
                    var sb = new StringBuilder(line)
                    {
                        [y + sy] = copy ? '@' : '#'
                    };
                    field[x - sx] = sb.ToString();
                }
            }
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var cycleSize = 5 * 7 * input[0].Length;
            var result = SolvePuzzle1(input, cycleSize);

            var iterations = 1_000_000_000_000;
            var cycles = iterations / cycleSize;
            var leftover = iterations % cycleSize;

            var total = (result - 7) * cycles + SolvePuzzle1(input, (int)leftover);

            _helper.WriteLine($"CycleSize: {cycleSize}");
            _helper.WriteLine($"Cycles: {cycles}");
            _helper.WriteLine($"Leftover: {leftover}");

            for (var i = 1; i < 10; i++)
            {
                _helper.WriteLine($"{i}: {(result - 7) * i + 7}, {SolvePuzzle1(input, cycleSize * i)}");
            }

            Assert.Equal(Results.Setup2, total);
        }

        [Fact]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var cycleSize = 5 * input[0].Length;
            var result = SolvePuzzle1(input, cycleSize);

            var iterations = 1_000_000_000_000;
            var cycles = iterations / cycleSize;
            var leftover = iterations % cycleSize;

            var total = (result - 7) * cycles + SolvePuzzle1(input, (int)leftover);

            _helper.WriteLine($"CycleSize: {cycleSize}");
            _helper.WriteLine($"Cycles: {cycles}");
            _helper.WriteLine($"Leftover: {leftover}");

            foreach (var i in new[] { 71, 73, 79, 83, 89, 97, 101 })
            {
                _helper.WriteLine("=======================");
                _helper.WriteLine($"{i}: {(result - 7) * i + 7}, {SolvePuzzle1(input, cycleSize * i)}");
                _helper.WriteLine($"{i * 2}: {(result - 7) * i * 2 + 7}, {SolvePuzzle1(input, cycleSize * i * 2)}");
            }

            Assert.Equal(Results.Puzzle2, total);
        }

        #endregion
    }
}
