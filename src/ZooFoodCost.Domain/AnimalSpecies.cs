namespace ZooFoodCost.Domain;
// I could simplify this by eliminating the FoodType Enums and only use MeatPercentage to define the food type. 0==fruit only, 100==meat only, between is a mix.
public record AnimalSpecies(
    string Name,
    decimal Rate,
    FoodType FoodType,
    decimal? MeatPercentage = null);
