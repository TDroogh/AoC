namespace AoC.Year2021.Day17
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 45;
            public const int Puzzle1 = 5671;
            public const int Setup2 = 112;
            public const int Puzzle2 = 4556;
        }

        public class Target
        {
            public int FromX { get; set; }
            public int ToX { get; set; }
            public int FromY { get; set; }
            public int ToY { get; set; }

            public static Target Parse(string line)
            {
                line = line.Replace("target area: x=", "");
                var split = line.Split(", y=");
                var splitX = split[0].Split("..");
                var splitY = split[1].Split("..");
                return new Target
                {
                    FromX = int.Parse(splitX[0]),
                    ToX = int.Parse(splitX[1]),
                    FromY = int.Parse(splitY[0]),
                    ToY = int.Parse(splitY[1]),
                };
            }

            public int GetMinimumXVelocity()
            {
                var cum = 0;
                for (var i = 0; i < 1000; i++)
                {
                    cum += i;
                    if (cum > FromX)
                        return i;
                }

                return 1000;
            }

            public int GetMinimumYVelocity()
            {
                return Math.Min(ToY, FromY);
            }

            public bool TryGetHit(int dx, int dy, out int maxY)
            {
                maxY = 0;
                var initDy = dy;
                if (dx > Math.Max(ToX, FromX) || dy < Math.Min(FromY, ToY))
                    return false;

                var x = 0;
                var y = 0;
                var i = 0;

                while (i < 1_000)
                {
                    x += dx;
                    y += dy;

                    //the probe's x velocity changes by 1 toward the value 0; that is,
                    //it decreases by 1 if it is greater than 0,
                    //increases by 1 if it is less than 0,
                    //or does not change if it is already 0.
                    dx -= dx == 0 ? 0 : dx > 0 ? 1 : -1;
                    dy -= 1;

                    if (x >= FromX && y <= ToY && x <= ToX && y >= FromY)
                    {
                        maxY = initDy * (initDy + 1) / 2;
                        return true;
                    }

                    if (x > ToX || y < FromY)
                        return false;

                    i++;
                }

                return false;
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return $"Target area: x={FromX}..{ToX}, y={FromY}..{ToY}";
            }
        }

        #region Puzzle 1

        private int SolvePuzzle1(string[] input)
        {
            var target = Target.Parse(input[0]);
            Console.WriteLine(target);

            var minX = target.GetMinimumXVelocity();
            var minY = target.GetMinimumYVelocity();

            Console.WriteLine($"Minimal x: {minX}, minimal y {minY}");
            var maxY = -1;
            for (var dx = minX; dx <= Math.Max(target.ToX, target.ToY); dx++)
            {
                var subMaxY = -1;
                for (var dy = minY; dy <= 110; dy++)
                {
                    if (target.TryGetHit(dx, dy, out var max) && max > subMaxY)
                    {
                        subMaxY = max;
                        Console.WriteLine($"velocity x: {dx}, velocity y {dy}: hit with max y {max}");
                    }
                    else
                    {
                        Console.WriteLine($"velocity x: {dx}, velocity y {dy}: no hit");
                    }
                }

                if (subMaxY >= maxY)
                {
                    maxY = subMaxY;
                }
                else if (maxY >= 0 && subMaxY >= 0)
                    return maxY;
            }

            return maxY;
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
            var target = Target.Parse(input[0]);
            Console.WriteLine(target);

            var minX = target.GetMinimumXVelocity();
            var minY = target.GetMinimumYVelocity();

            Console.WriteLine($"Minimal x: {minX}, minimal y {minY}");
            var hits = 0;
            for (var dx = minX; dx <= Math.Max(target.ToX, target.ToY); dx++)
            {
                for (var dy = minY; dy <= 110; dy++)
                {
                    if (target.TryGetHit(dx, dy, out var max))
                    {
                        hits++;
                        Console.WriteLine($"velocity x: {dx}, velocity y {dy}: hit with max y {max}");
                    }
                    //else
                    //{
                    //    Console.WriteLine($"velocity x: {dx}, velocity y {dy}: no hit");
                    //}
                }
            }

            return hits;
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