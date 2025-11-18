using System.IO;
using Moq;
using ZooFoodCost.Application;
using ZooFoodCost.Domain;
using ZooFoodCost.Infrastructure;
using Xunit;

namespace ZooFoodCost.Tests;

public class ZooServiceTests
{
    [Fact]
    public void CalculateTotalDailyCost_ValidInputs_ReturnsCost()
    {
        // Arrange
        var priceParserMock = new Mock<IFileParser<FoodPrice>>();
        var speciesParserMock = new Mock<IFileParser<AnimalSpecies>>();
        var zooParserMock = new Mock<IZooXmlParser>();
        var calculatorMock = new Mock<IDailyCostCalculator>();

        var zooService = new ZooService(
            priceParserMock.Object,
            speciesParserMock.Object,
            zooParserMock.Object,
            calculatorMock.Object
        );

        var foodPrice = new FoodPrice(12.56m, 5.60m);
        var species = new List<AnimalSpecies> { new AnimalSpecies("Lion", 0.10m, FoodType.Meat) };
        var animals = new List<ZooAnimal> { new ZooAnimal("Simba", 160m, species[0]) };
        var expectedCost = 201.00m;

        priceParserMock.Setup(p => p.Parse("prices.txt")).Returns(new[] { foodPrice });
        speciesParserMock.Setup(p => p.Parse("animals.csv")).Returns(species);
        zooParserMock.Setup(p => p.Parse("zoo.xml", species)).Returns(animals);
        calculatorMock.Setup(c => c.CalculateTotalDailyCost(animals, foodPrice)).Returns(expectedCost);

        // Act
        var result = zooService.CalculateTotalDailyCost("prices.txt", "animals.csv", "zoo.xml");

        // Assert
        Assert.Equal(expectedCost, result);
    }

    [Fact]
    public void CalculateTotalDailyCost_InvalidFilePath_ThrowsException()
    {
        // Arrange
        var priceParserMock = new Mock<IFileParser<FoodPrice>>();
        var zooService = new ZooService(priceParserMock.Object, null!, null!, null!);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            zooService.CalculateTotalDailyCost(null!, "animals.csv", "zoo.xml"));
    }
}