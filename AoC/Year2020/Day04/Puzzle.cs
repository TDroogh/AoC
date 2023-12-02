namespace AoC.Year2020.Day04
{
    [TestClass]
    public class Puzzle
    {
        private record Passport
        {
            public string YearOfBirth { get; set; } = default!;
            public string YearOfIssuing { get; set; } = default!;
            public string YearOfExpiration { get; set; } = default!;
            public string Height { get; set; } = default!;
            public string HairColor { get; set; } = default!;
            public string EyeColor { get; set; } = default!;
            public string PassportId { get; set; } = default!;

            public void SetProperty(string prop, string value)
            {
                switch (prop)
                {
                    case "byr":
                        YearOfBirth = value;
                        return;
                    case "iyr":
                        YearOfIssuing = value;
                        return;
                    case "eyr":
                        YearOfExpiration = value;
                        return;
                    case "hgt":
                        Height = value;
                        return;
                    case "hcl":
                        HairColor = value;
                        return;
                    case "ecl":
                        EyeColor = value;
                        return;
                    case "pid":
                        PassportId = value;
                        return;
                    case "cid":
                        return;
                }
            }

            private static bool ValidateYear(string year, int minvalue, int maxvalue)
            {
                if (int.TryParse(year, out var yearInt) == false)
                    return false;
                return yearInt >= minvalue && yearInt <= maxvalue;
            }

            private bool ValidateHeight()
            {
                var inches = Height.EndsWith("in", StringComparison.Ordinal);
                var cm = Height.EndsWith("cm", StringComparison.Ordinal);

                if (!inches && !cm)
                    return false;
                var value = int.Parse(Height.Substring(0, Height.Length - 2));

                if (inches)
                    return value >= 59 && value <= 76;
                return value >= 150 && value <= 193;
            }

            private bool ValidatePassportId()
            {
                return PassportId.Length == 9 && PassportId.All(char.IsDigit);
            }

            private bool ValidateHairColor()
            {
                return HairColor.Length == 7
                       && HairColor[0] == '#'
                       && HairColor.Skip(1)
                           .All(x => char.IsDigit(x)
                                     || x == 'a'
                                     || x == 'b'
                                     || x == 'c'
                                     || x == 'd'
                                     || x == 'e'
                                     || x == 'f');
            }

            private bool ValidateEyeColor()
            {
                return EyeColor == "amb"
                       || EyeColor == "blu"
                       || EyeColor == "brn"
                       || EyeColor == "gry"
                       || EyeColor == "grn"
                       || EyeColor == "hzl"
                       || EyeColor == "oth";
            }

            public bool IsValid(bool validate)
            {
                if (!IsComplete())
                    return false;

                if (validate)
                {
                    try
                    {
                        return ValidateYear(YearOfBirth, 1920, 2002)
                               && ValidateYear(YearOfIssuing, 2010, 2020)
                               && ValidateYear(YearOfExpiration, 2020, 2030)
                               && ValidateHeight()
                               && ValidatePassportId()
                               && ValidateHairColor()
                               && ValidateEyeColor();
                    }
                    catch
                    {
                        return false;
                    }
                }

                return true;
            }

            public bool IsComplete()
            {
                return string.IsNullOrEmpty(YearOfBirth) == false
                       && string.IsNullOrEmpty(YearOfIssuing) == false
                       && string.IsNullOrEmpty(YearOfExpiration) == false
                       && string.IsNullOrEmpty(Height) == false
                       && string.IsNullOrEmpty(HairColor) == false
                       && string.IsNullOrEmpty(EyeColor) == false
                       && string.IsNullOrEmpty(PassportId) == false;
            }
        }

        private int ParsePassports(string[] input, bool validate)
        {
            var count = 0;
            var passport = new Passport();
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (passport.IsValid(validate))
                        count++;

                    passport = new Passport();
                    continue;
                }

                foreach (var prop in line.Split(" "))
                {
                    var split = prop.Split(":");
                    passport.SetProperty(split[0], split[1]);
                }
            }

            if (passport.IsValid(validate))
                count++;

            return count;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput("setup1");
            Assert.AreEqual(2, ParsePassports(input, false));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(192, ParsePassports(input, false));
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput("setup2");
            Assert.AreEqual(4, ParsePassports(input, true));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(101, ParsePassports(input, true));
        }
    }
}