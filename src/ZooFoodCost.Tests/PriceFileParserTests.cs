using System.IO;
using ZooFoodCost.Domain;
using ZooFoodCost.Infrastructure;
using Xunit;

namespace ZooFoodCost.Tests;

public class PriceFileParserTests
{
    [Fact]
    public void Parse_ValidFile_ReturnsPrices()
    {
        // Arrange
        var parser = new PriceFileParser();
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "Meat=12.56\nFruit=5.60");

        // Act
        var result = parser.Parse(tempFile).ToList();

        // Assert
        Assert.Single(result);
        var price = result[0];
        Assert.Equal(12.56m, price.MeatPrice);
        Assert.Equal(5.60m, price.FruitPrice);

        // Cleanup
        File.Delete(tempFile);
    }

    [Fact]
    public void Parse_FileNotFound_ThrowsException()
    {
        // Arrange
        var parser = new PriceFileParser();

        // Act & Assert
        var exception = Assert.Throws<ZooFoodCostException>(() =>
            parser.Parse("nonexistent.txt").ToList());
        Assert.Contains("Prices file not found", exception.Message);
    }
}
