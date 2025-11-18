using ZooFoodCost.Application;
using ZooFoodCost.Domain;
using Xunit;

namespace ZooFoodCost.Tests;

public class DailyCostCalculatorTests
{
    [Fact]
    public void CalculateTotalDailyCost_Carnivore_ReturnsCorrectCost()
    {
        // Arrange
        var calculator = new DailyCostCalculator();
        var lionSpecies = new AnimalSpecies("Lion", 0.10m, FoodType.Meat);
        var lion = new ZooAnimal("Simba", 160m, lionSpecies);
        var animals = new List<ZooAnimal> { lion };
        var foodPrice = new FoodPrice(12.56m, 5.60m);

        // Act
        var result = calculator.CalculateTotalDailyCost(animals, foodPrice);

        // Assert
        // 160kg * 0.10 rate = 16kg meat * €12.56/kg = €200.96 (rounded)
        Assert.Equal(200.96m, result);
    }

    [Fact]
    public void CalculateTotalDailyCost_Herbivore_ReturnsCorrectCost()
    {
        // Arrange
        var calculator = new DailyCostCalculator();
        var giraffeSpecies = new AnimalSpecies("Giraffe", 0.08m, FoodType.Fruit);
        var giraffe = new ZooAnimal("Hanna", 200m, giraffeSpecies);
        var animals = new List<ZooAnimal> { giraffe };
        var foodPrice = new FoodPrice(12.56m, 5.60m);

        // Act
        var result = calculator.CalculateTotalDailyCost(animals, foodPrice);

        // Assert
        // 200kg * 0.08 rate = 16kg fruit * €5.60/kg = €89.60
        Assert.Equal(89.60m, result);
    }

    [Fact]
    public void CalculateTotalDailyCost_MultipleAnimals_ReturnsTotal()
    {
        // Arrange
        var calculator = new DailyCostCalculator();
        var lionSpecies = new AnimalSpecies("Lion", 0.10m, FoodType.Meat);
        var giraffeSpecies = new AnimalSpecies("Giraffe", 0.08m, FoodType.Fruit);
        var animals = new List<ZooAnimal> {
            new ZooAnimal("Simba", 160m, lionSpecies),
            new ZooAnimal("Hanna", 200m, giraffeSpecies)
        };
        var foodPrice = new FoodPrice(12.56m, 5.60m);

        // Act
        var result = calculator.CalculateTotalDailyCost(animals, foodPrice);

        // Assert
        // Lion: 200.96 + Giraffe: 89.60 = 290.56
        Assert.Equal(290.56m, result);
    }
}