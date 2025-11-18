using System.IO;
using ZooFoodCost.Domain;
using ZooFoodCost.Infrastructure;
using Xunit;

namespace ZooFoodCost.Tests;

public class AnimalSpeciesParserTests
{
    [Fact]
    public void Parse_ValidFile_ReturnsSpecies()
    {
        // Arrange
        var parser = new AnimalSpeciesParser();
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "Lion;0.10;meat;\nTiger;0.09;meat;");

        // Act
        var result = parser.Parse(tempFile).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        var lion = result.First(s => s.Name == "Lion");
        Assert.Equal(0.10m, lion.Rate);
        Assert.Equal(FoodType.Meat, lion.FoodType);

        // Cleanup
        File.Delete(tempFile);
    }

    [Fact]
    public void Parse_InvalidFile_ThrowsException()
    {
        // Arrange
        var parser = new AnimalSpeciesParser();
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "Invalid;name;meat");

        // Act & Assert
        Assert.Throws<FormatException>(() =>
            parser.Parse(tempFile).ToList());

        // Cleanup
        File.Delete(tempFile);
    }
}
