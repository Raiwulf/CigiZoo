using System.IO;
using ZooFoodCost.Domain;
using ZooFoodCost.Infrastructure;
using Xunit;
using System.Linq;

namespace ZooFoodCost.Tests;

public class ZooXmlParserTests
{
    private readonly List<AnimalSpecies> _testSpecies;

    public ZooXmlParserTests()
    {
        // Setup test species data
        _testSpecies = new List<AnimalSpecies>
        {
            new AnimalSpecies("Lion", 0.10m, FoodType.Meat),
            new AnimalSpecies("Tiger", 0.09m, FoodType.Meat),
            new AnimalSpecies("Giraffe", 0.08m, FoodType.Fruit),
            new AnimalSpecies("Wolf", 0.07m, FoodType.Both, 90m)
        };
    }

    [Fact]
    public void Parse_MultipleAnimalContainers_ReturnsAllAnimals()
    {
        // Arrange
        var parser = new ZooXmlParser();
        var tempFile = Path.GetTempFileName();
        var xmlContent = @"<Zoo>
  <Lions>
    <Lion name='Simba' kg='160'/>
  </Lions>
  <Giraffes>
    <Giraffe name='Hanna' kg='200'/>
  </Giraffes>
</Zoo>";
        File.WriteAllText(tempFile, xmlContent);

        // Act
        var result = parser.Parse(tempFile, _testSpecies).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        var simba = result.First(a => a.Name == "Simba");
        var hanna = result.First(a => a.Name == "Hanna");

        Assert.Equal(160m, simba.Weight);
        Assert.Equal("Lion", simba.Species.Name);
        Assert.Equal(200m, hanna.Weight);
        Assert.Equal("Giraffe", hanna.Species.Name);

        // Cleanup
        File.Delete(tempFile);
    }

    [Fact]
    public void Parse_InvalidXml_ThrowsException()
    {
        // Arrange
        var parser = new ZooXmlParser();
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "<Animals></Animals>"); // Wrong root element. should've been <Zoo></Zoo>

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            parser.Parse(tempFile, _testSpecies).ToList());

        // Cleanup
        File.Delete(tempFile);
    }

    [Fact]
    public void Parse_FileNotFound_ThrowsException()
    {
        // Arrange
        var parser = new ZooXmlParser();

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() =>
            parser.Parse("idontexist.xml", _testSpecies).ToList());
    }

    [Fact]
    public void Parse_UnknownSpecies_ThrowsZooFoodCostException()
    {
        // Arrange
        var parser = new ZooXmlParser();
        var tempFile = Path.GetTempFileName();
        var xmlContent = @"<Zoo>
  <Cobras>
    <Cobra name='Slither' kg='2'/>
  </Cobras>
</Zoo>";
        File.WriteAllText(tempFile, xmlContent);

        // Act & Assert
        var exception = Assert.Throws<ZooFoodCostException>(() =>
            parser.Parse(tempFile, _testSpecies).ToList());

        Assert.Contains("Unknown animal species 'Cobra'", exception.Message);
        Assert.Contains("Please add species data to animals.csv", exception.Message);

        // Cleanup
        File.Delete(tempFile);
    }
}