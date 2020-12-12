using System.Linq;
using AoC.Inputs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020
{
    [TestClass]
    public class Day4
    {
        private class Passport
        {
            public string YearOfBirth { get; set; }
            public string YearOfIssuing { get; set; }
            public string YearOfExpiration { get; set; }
            public string Height { get; set; }
            public string HairColor { get; set; }
            public string EyeColor { get; set; }
            public string PassportId { get; set; }
            public string CountryId { get; set; }

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
                        CountryId = value;
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
                var inches = Height.EndsWith("in");
                var cm = Height.EndsWith("cm");

                if(!inches && !cm) return false;
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
                if (!IsComplete()) return false;

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

        private int ParsePassports(int puzzle, bool validate)
        {
            var input = InputReader.ReadInput(2020, 4, puzzle);
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
            Assert.AreEqual(2, ParsePassports(0, false));
        }

        [TestMethod]
        public void Puzzle1()
        {
            Assert.AreEqual(192, ParsePassports(1, false));
        }
        
        [TestMethod]
        public void Setup2()
        {
            Assert.AreEqual(4, ParsePassports(2, true));
        }
        
        [TestMethod]
        public void Puzzle2()
        {
            Assert.AreEqual(101, ParsePassports(1, true));
        }
    }
}