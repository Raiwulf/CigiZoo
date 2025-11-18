namespace ZooFoodCost.Domain;

public class ZooFoodCostException : Exception
{
    public ZooFoodCostException(string message) : base(message) { }
    public ZooFoodCostException(string message, Exception innerException) : base(message, innerException) { }
}
