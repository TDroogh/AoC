namespace AoC.Year2021.Day07
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 37;
            public const int Puzzle1 = 347509;
            public const int Setup2 = 168;
            public const int Puzzle2 = 98257206;
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var positions = input[0].Split(",").Select(int.Parse).ToArray();
            var leastFuel = int.MaxValue;

            for (var i = positions.Min(); i <= positions.Max(); i++)
            {
                var fuel = 0;
                foreach (var pos in positions)
                {
                    fuel += Math.Abs(pos - i);
                }

                if (fuel < leastFuel)
                    leastFuel = fuel;
            }

            return leastFuel;
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
            var positions = input[0].Split(",").Select(int.Parse).ToArray();
            var leastFuel = int.MaxValue;

            for (var i = positions.Min(); i <= positions.Max(); i++)
            {
                var fuel = 0;
                foreach (var pos in positions)
                {
                    var diff = Math.Abs(pos - i);
                    fuel += diff * (diff + 1) / 2;
                }

                if (fuel < leastFuel)
                    leastFuel = fuel;
            }

            return leastFuel;
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