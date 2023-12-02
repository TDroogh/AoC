namespace AoC.Year2015.Day02
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private int CalculateSurface(string[] input)
        {
            var totalSum = 0;

            foreach (var line in input)
            {
                var split = line.Split("x");
                var l = int.Parse(split[0]);
                var w = int.Parse(split[1]);
                var h = int.Parse(split[2]);

                totalSum += CalculateSurface(l, w, h);
            }

            return totalSum;
        }

        private int CalculateSurface(int l, int w, int h)
        {
            var lw = l * w;
            var wh = w * h;
            var hl = h * l;

            return lw * 2 + wh * 2 + hl * 2 + Math.Min(lw, Math.Min(wh, hl));
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(101, CalculateSurface(input));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(1606483, CalculateSurface(input));
        }

        #endregion

        #region Puzzle 2

        private int CalculateRibbon(string[] input)
        {
            var totalSum = 0;

            foreach (var line in input)
            {
                var split = line.Split("x");
                var l = int.Parse(split[0]);
                var w = int.Parse(split[1]);
                var h = int.Parse(split[2]);

                totalSum += CalculateRibbon(l, w, h);
            }

            return totalSum;
        }

        private int CalculateRibbon(params int[] l)
        {
            return l.OrderBy(x => x).Take(2).Sum() * 2 + l.Aggregate((p, n) => p * n);
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(48, CalculateRibbon(input));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(3842356, CalculateRibbon(input));
        }

        #endregion
    }
}