using ZooFoodCost.Domain;

namespace ZooFoodCost.Application;

public class DailyCostCalculator : IDailyCostCalculator
{
    public decimal CalculateTotalDailyCost(IEnumerable<ZooAnimal> animals, FoodPrice foodPrice)
    {
        return animals.Sum(animal => CalculateAnimalCost(animal, foodPrice));
    }

    private static decimal CalculateAnimalCost(ZooAnimal animal, FoodPrice foodPrice)
    {
        decimal foodNeeded = animal.Weight * animal.Species.Rate;
        decimal meatNeeded = 0;
        decimal fruitNeeded = 0;

        switch (animal.Species.FoodType)
        {
            case FoodType.Meat:
                meatNeeded = foodNeeded;
                break;

            case FoodType.Fruit:
                fruitNeeded = foodNeeded;
                break;

            case FoodType.Both:
                var meatPercentage = animal.Species.MeatPercentage ?? 0;
                meatNeeded = foodNeeded * (meatPercentage / 100m);
                fruitNeeded = foodNeeded * (1 - meatPercentage / 100m);
                break;
        }

        return (meatNeeded * foodPrice.MeatPrice) + (fruitNeeded * foodPrice.FruitPrice);
    }
}
