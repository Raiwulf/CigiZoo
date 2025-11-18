using System.IO;
using ZooFoodCost.Application;
using ZooFoodCost.Domain;
using ZooFoodCost.Infrastructure;
using Xunit;

namespace ZooFoodCost.Tests;

public class DataAdaptationTests
{
    [Fact]
    public void ZooService_WithCompletelyNewData_CalculatesCorrectTotalCost()
    {
        // Arrange - Create completely new embedded data
        var newPricesContent = "Meat=8.75\nFruit=3.25";
        var newAnimalsContent = "Elephant;0.12;fruit;\nBear;0.15;both;75%\nSnake;0.03;meat;\nMonkey;0.06;fruit;\nEagle;0.04;both;60%";
        var newZooContent = @"<Zoo>
  <Elephants>
    <Elephant name='Jumbo' kg='500'/>
    <Elephant name='Ellie' kg='480'/>
  </Elephants>
  <Bears>
    <Bear name='Bruno' kg='200'/>
    <Bear name='Bella' kg='180'/>
  </Bears>
  <Snakes>
    <Snake name='Sly' kg='10'/>
  </Snakes>
  <Monkeys>
    <Monkey name='Max' kg='15'/>
    <Monkey name='Mia' kg='12'/>
  </Monkeys>
  <Eagles>
    <Eagle name='Eddie' kg='8'/>
    <Eagle name='Ella' kg='7'/>
  </Eagles>
</Zoo>";

        // Expected calculation breakdown:
        // Elephants: Jumbo(500*0.12*3.25=195), Ellie(480*0.12*3.25=187.20) = 382.20
        // Bears: Bruno(200*0.1125*8.75 + 200*0.0375*3.25 = 196.875 + 24.375 = 221.25),
        //        Bella(180*0.1125*8.75 + 180*0.0375*3.25 = 177.1875 + 21.9375 = 199.125) = 420.375
        // Snakes: Sly(10*0.03*8.75 = 2.625)
        // Monkeys: Max(15*0.06*3.25 = 2.925), Mia(12*0.06*3.25 = 2.34) = 5.265
        // Eagles: Eddie(8*0.024*8.75 + 8*0.016*3.25 = 1.68 + 0.416 = 2.096),
        //         Ella(7*0.024*8.75 + 7*0.016*3.25 = 1.47 + 0.364 = 1.834) = 3.93
        // Total: 382.20 + 420.38 + 2.63 + 5.27 + 3.93 = 814.39
        var expectedTotalCost = 814.395m;

        var pricesFile = Path.GetTempFileName();
        var animalsFile = Path.GetTempFileName();
        var zooFile = Path.GetTempFileName();

        try
        {
            File.WriteAllText(pricesFile, newPricesContent);
            File.WriteAllText(animalsFile, newAnimalsContent);
            File.WriteAllText(zooFile, newZooContent);

            var priceParser = new PriceFileParser();
            var speciesParser = new AnimalSpeciesParser();
            var zooParser = new ZooXmlParser();
            var calculator = new DailyCostCalculator();

            var zooService = new ZooService(priceParser, speciesParser, zooParser, calculator);

            // Act
            var result = zooService.CalculateTotalDailyCost(pricesFile, animalsFile, zooFile);

            // Assert
            Assert.Equal(expectedTotalCost, result);
        }
        finally
        {
            // Cleanup
            File.Delete(pricesFile);
            File.Delete(animalsFile);
            File.Delete(zooFile);
        }
    }


    [Fact]
    public void ZooService_WithMixedCaseAndWhitespaceInNewData_HandlesCorrectly()
    {
        // Arrange - Test with data that has mixed case and extra whitespace
        var newPricesContent = "  Meat  =  15.50  \n  Fruit  =  7.25  ";
        var newAnimalsContent = "PANTHER;0.11;meat;\n  dolphin  ;0.09;both;80%\n";
        var newZooContent = @"<Zoo>
  <PANTHERS>
    <PANTHER name='Shadow' kg='120'/>
  </PANTHERS>
  <DOLPHINS>
    <DOLPHIN name='Flipper' kg='85'/>
  </DOLPHINS>
</Zoo>";

        // Expected calculation:
        // Panther: 120 * 0.11 * 15.50 = 204.6
        // Dolphin: 85 * 0.09 * 0.80 * 15.50 (meat) + 85 * 0.09 * 0.20 * 7.25 (fruit) = 94.86 + 11.0925 = 105.9525
        // Total: 310.5525
        var expectedTotalCost = 310.5525m;

        var pricesFile = Path.GetTempFileName();
        var animalsFile = Path.GetTempFileName();
        var zooFile = Path.GetTempFileName();

        try
        {
            File.WriteAllText(pricesFile, newPricesContent);
            File.WriteAllText(animalsFile, newAnimalsContent);
            File.WriteAllText(zooFile, newZooContent);

            var priceParser = new PriceFileParser();
            var speciesParser = new AnimalSpeciesParser();
            var zooParser = new ZooXmlParser();
            var calculator = new DailyCostCalculator();

            var zooService = new ZooService(priceParser, speciesParser, zooParser, calculator);

            // Act
            var result = zooService.CalculateTotalDailyCost(pricesFile, animalsFile, zooFile);

            // Assert
            Assert.Equal(expectedTotalCost, result);
        }
        finally
        {
            // Cleanup
            File.Delete(pricesFile);
            File.Delete(animalsFile);
            File.Delete(zooFile);
        }
    }
}
