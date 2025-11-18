# CigiZoo

```pwsh
git clone https://github.com/Raiwulf/CigiZoo.git
cd CigiZoo
```
```pwsh
# Run API
cd src/ZooFoodCost.Api && dotnet run
```
```pwsh
# Run Console App
cd src/ZooFoodCost.Console && dotnet run
```

```pwsh
# Use custom prices file
cd src/ZooFoodCost.Console && dotnet run --prices "C:\custom-prices.txt"

# Use custom animals and zoo files
cd src/ZooFoodCost.Console && dotnet run --animals "C:\my-animals.csv" --zoo "C:\my-zoo.xml"

# Short form args
cd src/ZooFoodCost.Console && dotnet run -p "prices.txt" -a "animals.csv" -z "zoo.xml"

# only override what you need
cd src/ZooFoodCost.Console && dotnet run --prices "C:\custom-prices.txt"
```
