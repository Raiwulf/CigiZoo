namespace ZooFoodCost.Domain;

public interface IZooXmlParser
{
    IEnumerable<ZooAnimal> Parse(string filePath, IEnumerable<AnimalSpecies> species);
}
