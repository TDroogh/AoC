namespace AoC.Year2020.Day21
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        public class Recipe
        {
            public required string[] Ingredients { get; init; }
            public required string[] Allergens { get; init; }

            public static Recipe Parse(string input)
            {
                var split = input.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                var ingredients = split[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var allergens = split[1].Split(new[] { "contains ", ", " }, StringSplitOptions.RemoveEmptyEntries);

                return new Recipe
                {
                    Ingredients = ingredients,
                    Allergens = allergens
                };
            }
        }

        private int SolvePuzzle1(string[] input)
        {
            var recipes = input.Select(Recipe.Parse).ToList();
            var allIngredients = recipes.SelectMany(x => x.Ingredients).Distinct().ToList();
            var allAllergens = recipes.SelectMany(x => x.Allergens).Distinct().ToList();

            var totalCount = 0;

            foreach (var ingredient in allIngredients)
            {
                var count = 0;
                var possibleAllergens = allAllergens.ToList();
                foreach (var recipe in recipes)
                {
                    if (recipe.Ingredients.Contains(ingredient) == false)
                        //possibleAllergens = possibleAllergens.Intersect(recipe.Allergens).ToList();
                        //else
                        possibleAllergens = possibleAllergens.Except(recipe.Allergens).ToList();
                    else
                        count++;
                }

                if (possibleAllergens.Any() == false)
                    totalCount += count;
            }

            return totalCount;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(1685, result);
        }

        #endregion

        #region Puzzle 2

        private string SolvePuzzle2(string[] input)
        {
            var recipes = input.Select(Recipe.Parse).ToList();
            var allIngredients = recipes.SelectMany(x => x.Ingredients).Distinct().ToList();
            var allAllergens = recipes.SelectMany(x => x.Allergens).Distinct().ToList();

            var map = new Dictionary<string, string?>();

            var i = 0;
            while (map.Count != allIngredients.Count)
            {
                Assert.AreNotEqual(1000, i++);

                foreach (var ingredient in allIngredients.Where(x => map.ContainsKey(x) == false))
                {
                    var possibleAllergens = allAllergens.Where(x => map.Values.Contains(x) == false).ToList();
                    foreach (var recipe in recipes)
                    {
                        if (recipe.Ingredients.Contains(ingredient) == false)
                            possibleAllergens = possibleAllergens.Except(recipe.Allergens).ToList();
                    }

                    if (possibleAllergens.Count == 0)
                        map.Add(ingredient, null);
                    if (possibleAllergens.Count == 1)
                        map.Add(ingredient, possibleAllergens.Single());
                }
            }

            var mappedValues = map.Where(x => x.Value != null).OrderBy(x => x.Value).Select(x => x.Key);
            return string.Join(",", mappedValues);
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual("mxmxvkd,sqjhc,fvjkl", result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual("ntft,nhx,kfxr,xmhsbd,rrjb,xzhxj,chbtp,cqvc", result);
        }

        #endregion
    }
}