namespace AoC.Year2020.Day08
{
    [TestClass]
    public class Puzzle
    {
        private int CalculateAcc(string[] input)
        {
            var acc = 0;
            var handledIndices = new List<int>();
            var i = 0;

            while (i < input.Length)
            {
                if (handledIndices.Contains(i))
                    break;
                handledIndices.Add(i);

                var line = input[i];
                var split = line.Split(" ");

                var action = split[0];
                if (action == "nop")
                {
                    i++;
                }
                else if (action == "acc")
                {
                    i++;
                    acc += int.Parse(split[1]);
                }
                else if (action == "jmp")
                {
                    i += int.Parse(split[1]);
                }
            }

            return acc;
        }

        private int CalculateAccFixed(string[] input)
        {
            var fix = 0;

            while (true)
            {
                var acc = 0;
                var handledIndices = new List<int>();
                var i = 0;

                while (true)
                {
                    if (i >= input.Length)
                    {
                        return acc;
                    }

                    if (handledIndices.Contains(i))
                        break;
                    handledIndices.Add(i);

                    var line = input[i];
                    var split = line.Split(" ");

                    var action = split[0];
                    if (i == fix)
                    {
                        if (action == "nop")
                            action = "jmp";
                        if (action == "jmp")
                            action = "nop";
                    }

                    if (action == "nop")
                    {
                        i++;
                    }
                    else if (action == "acc")
                    {
                        i++;
                        acc += int.Parse(split[1]);
                    }
                    else if (action == "jmp")
                    {
                        i += int.Parse(split[1]);
                    }
                }

                fix++;
            }
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(5, CalculateAcc(input));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(1859, CalculateAcc(input));
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(8, CalculateAccFixed(input));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(1235, CalculateAccFixed(input));
        }
    }
}
