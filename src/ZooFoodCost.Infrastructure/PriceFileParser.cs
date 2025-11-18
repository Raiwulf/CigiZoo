using ZooFoodCost.Domain;

namespace ZooFoodCost.Infrastructure;

public class PriceFileParser : IFileParser<FoodPrice>
{
    public IEnumerable<FoodPrice> Parse(string filePath)
    {
        if (!File.Exists(filePath))
            throw new ZooFoodCostException($"Prices file not found: {filePath}");

        var lines = File.ReadAllLines(filePath);
        decimal meatPrice = 0, fruitPrice = 0;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split('=');
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                decimal price = decimal.Parse(value);

                if (key == "Meat") meatPrice = price;
                else if (key == "Fruit") fruitPrice = price;
            }
        }

        yield return new FoodPrice(meatPrice, fruitPrice);
    }
}
