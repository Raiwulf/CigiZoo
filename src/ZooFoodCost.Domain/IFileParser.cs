namespace ZooFoodCost.Domain;

public interface IFileParser<T>
{
    IEnumerable<T> Parse(string filePath);
}
