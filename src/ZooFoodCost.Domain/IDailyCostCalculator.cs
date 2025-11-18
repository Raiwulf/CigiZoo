namespace ZooFoodCost.Domain;

public interface IDailyCostCalculator
{
    decimal CalculateTotalDailyCost(IEnumerable<ZooAnimal> animals, FoodPrice foodPrice);
}
