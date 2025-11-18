using ZooFoodCost.Domain;

namespace ZooFoodCost.Application;

public class ZooService
{
    private readonly IFileParser<FoodPrice> _priceParser;
    private readonly IFileParser<AnimalSpecies> _speciesParser;
    private readonly IZooXmlParser _zooParser;
    private readonly IDailyCostCalculator _calculator;

    public ZooService(
        IFileParser<FoodPrice> priceParser,
        IFileParser<AnimalSpecies> speciesParser,
        IZooXmlParser zooParser,
        IDailyCostCalculator calculator)
    {
        _priceParser = priceParser;
        _speciesParser = speciesParser;
        _zooParser = zooParser;
        _calculator = calculator;
    }

    public decimal CalculateTotalDailyCost(string pricesFilePath, string speciesFilePath, string zooFilePath)
    {
        var foodPrice = _priceParser.Parse(pricesFilePath).Single();
        var species = _speciesParser.Parse(speciesFilePath).ToList();
        var animals = _zooParser.Parse(zooFilePath, species).ToList();
        return _calculator.CalculateTotalDailyCost(animals, foodPrice);
    }
}
