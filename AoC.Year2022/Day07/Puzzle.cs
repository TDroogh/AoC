namespace AoC.Year2022.Day07
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 95437;
            public const int Puzzle1 = 1778099;
            public const int Setup2 = 24933642;
            public const int Puzzle2 = 1623571;
        }

        private enum CommandType
        {
            ChangeDirectory,
            ListDirectory
        }

        private class Directory
        {
            public Directory? ParentDirectory { get; }

            public Dictionary<string, Directory> Directories { get; } = new Dictionary<string, Directory>();
            public Dictionary<string, int> Files { get; } = new Dictionary<string, int>();

            public Directory(Directory? parentDirectory)
            {
                ParentDirectory = parentDirectory;
            }

            public int GetTotalSize()
            {
                return Files.Values.Sum() + Directories.Values.Sum(dir => dir.GetTotalSize());
            }

            public IEnumerable<Directory> GetAllDirectories()
            {
                yield return this;

                foreach (var dir in Directories.Values.SelectMany(dir => dir.GetAllDirectories()))
                {
                    yield return dir;
                }
            }
        }

        private class Command
        {
            public CommandType Type { get; }
            public string? ChangeToDirectory { get; }
            public List<string> ListOutput { get; } = new List<string>();

            public Command(string changeToDirectory)
            {
                Type = CommandType.ChangeDirectory;
                ChangeToDirectory = changeToDirectory;
            }

            public Command()
            {
                Type = CommandType.ListDirectory;
            }
        }

        private static IEnumerable<Command> GetCommands(string[] input)
        {
            Command? command = null;

            foreach (var line in input)
            {
                if (line[0] == '$')
                {
                    if (command != null)
                    {
                        yield return command;
                    }

                    command = line[2..4] == "cd" ? new Command(line[5..]) : new Command();
                }
                else
                {
                    command?.ListOutput.Add(line);
                }
            }

            if (command != null)
            {
                yield return command;
            }
        }

        private static Directory ParseDirectories(string[] input)
        {
            var rootDirectory = new Directory(null);
            var currentDirectory = rootDirectory;

            foreach (var command in GetCommands(input).Skip(1))
            {
                switch (command.Type)
                {
                    case CommandType.ChangeDirectory:
                    {
                        currentDirectory = command.ChangeToDirectory == ".." ? currentDirectory.ParentDirectory! : currentDirectory.Directories[command.ChangeToDirectory!];
                        break;
                    }
                    case CommandType.ListDirectory:
                    {
                        foreach (var item in command.ListOutput)
                        {
                            if (item[..3] == "dir")
                            {
                                currentDirectory.Directories.Add(item[4..], new Directory(currentDirectory));
                            }
                            else
                            {
                                var split = item.Split(" ");
                                currentDirectory.Files.Add(split[1], int.Parse(split[0]));
                            }
                        }

                        break;
                    }
                }
            }

            return rootDirectory;
        }

        #region Puzzle 1

        private static int SolvePuzzle1(string[] input)
        {
            var rootDirectory = ParseDirectories(input);

            return rootDirectory.GetAllDirectories().Select(dir => dir.GetTotalSize()).Where(size => size < 100000).Sum();
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

        private static int SolvePuzzle2(string[] input)
        {
            var rootDirectory = ParseDirectories(input);
            var requiredSpace = rootDirectory.GetTotalSize() - 40_000_000;

            return rootDirectory.GetAllDirectories().Select(dir => dir.GetTotalSize()).Where(size => size >= requiredSpace).Min();
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
