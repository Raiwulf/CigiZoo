using ZooFoodCost.Domain;

namespace ZooFoodCost.Infrastructure;

public class AnimalSpeciesParser : IFileParser<AnimalSpecies>
{
    public IEnumerable<AnimalSpecies> Parse(string filePath)
    {
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line.Trim())) continue;

            var parts = line.Split(';');
            var name = parts[0];
            var rate = decimal.Parse(parts[1]);
            var foodTypeString = parts[2];
            var foodType = Enum.Parse<FoodType>(foodTypeString, true);

            decimal? meatPercentage = null;
            if (parts.Length == 4 && !string.IsNullOrEmpty(parts[3]))
            {
                var percentageString = parts[3].TrimEnd('%');
                meatPercentage = decimal.Parse(percentageString);
            }

            yield return new AnimalSpecies(name, rate, foodType, meatPercentage);
        }
    }
}
