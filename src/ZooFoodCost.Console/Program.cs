using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZooFoodCost.Application;
using ZooFoodCost.Domain;
using ZooFoodCost.Infrastructure;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IDailyCostCalculator, DailyCostCalculator>();
builder.Services.AddSingleton<IFileParser<FoodPrice>, PriceFileParser>();
builder.Services.AddSingleton<IFileParser<AnimalSpecies>, AnimalSpeciesParser>();
builder.Services.AddSingleton<IZooXmlParser, ZooXmlParser>();
builder.Services.AddSingleton<ZooService>();

using IHost host = builder.Build();

var zooService = host.Services.GetRequiredService<ZooService>();
var config = host.Services.GetRequiredService<IConfiguration>();

(string pricesFile, string speciesFile, string zooFile) = GetConfiguredFilePaths(config);

(pricesFile, speciesFile, zooFile) = ParseArguments(args, pricesFile, speciesFile, zooFile);

static (string pricesFile, string speciesFile, string zooFile) GetConfiguredFilePaths(IConfiguration config)
{
    string pricesFile = config["DataFiles:PricesFile"] ?? "D:\\Code\\repos\\cSharp\\CigiZoo\\sample-data\\prices.txt";
    string animalsFile = config["DataFiles:AnimalsFile"] ?? "D:\\Code\\repos\\cSharp\\CigiZoo\\sample-data\\animals.csv";
    string zooFile = config["DataFiles:ZooFile"] ?? "D:\\Code\\repos\\cSharp\\CigiZoo\\sample-data\\zoo.xml";

    return (pricesFile, animalsFile, zooFile);
}

static (string pricesFile, string speciesFile, string zooFile) ParseArguments(string[] args, string defaultPricesFile, string defaultSpeciesFile, string defaultZooFile)
{
    var pricesFile = defaultPricesFile;
    var speciesFile = defaultSpeciesFile;
    var zooFile = defaultZooFile;

    for (int i = 0; i < args.Length; i++)
    {
        switch (args[i])
        {
            case "--prices":
            case "-p":
                if (i + 1 < args.Length) pricesFile = args[++i];
                break;

            case "--animals":
            case "-a":
                if (i + 1 < args.Length) speciesFile = args[++i];
                break;

            case "--zoo":
            case "-z":
                if (i + 1 < args.Length) zooFile = args[++i];
                break;
        }
    }

    return (pricesFile, speciesFile, zooFile);
}

var totalCost = zooService.CalculateTotalDailyCost(pricesFile, speciesFile, zooFile);
Console.WriteLine($"Total daily food cost: €{totalCost}");

return 0;
