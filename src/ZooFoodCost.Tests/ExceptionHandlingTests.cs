using ZooFoodCost.Domain;
using Xunit;

namespace ZooFoodCost.Tests;

public class ExceptionHandlingTests
{
    [Fact]
    public void ZooFoodCostException_MessageConstructor_SetsMessage()
    {
        // Arrange
        var expectedMessage = "Test error message";

        // Act
        var exception = new ZooFoodCostException(expectedMessage);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

}