using Microsoft.Extensions.Configuration;
using ZooFoodCost.Application;
using ZooFoodCost.Domain;
using ZooFoodCost.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDailyCostCalculator, DailyCostCalculator>();
builder.Services.AddSingleton<IFileParser<FoodPrice>, PriceFileParser>();
builder.Services.AddSingleton<IFileParser<AnimalSpecies>, AnimalSpeciesParser>();
builder.Services.AddSingleton<IZooXmlParser, ZooXmlParser>();
builder.Services.AddSingleton<ZooService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/zoo/daily-cost", (ZooService zooService, IConfiguration config) =>
{
    var (pricesFile, animalsFile, zooFile) = GetConfiguredFilePaths(config);
    var totalCost = zooService.CalculateTotalDailyCost(pricesFile, animalsFile, zooFile);
    return Results.Ok(new { TotalDailyCost = totalCost });
});

static (string pricesFile, string animalsFile, string zooFile) GetConfiguredFilePaths(IConfiguration config)
{
    string pricesFile = config["DataFiles:PricesFile"] ?? "D:\\Code\\repos\\cSharp\\CigiZoo\\sample-data\\prices.txt";
    string animalsFile = config["DataFiles:AnimalsFile"] ?? "D:\\Code\\repos\\cSharp\\CigiZoo\\sample-data\\animals.csv";
    string zooFile = config["DataFiles:ZooFile"] ?? "D:\\Code\\repos\\cSharp\\CigiZoo\\sample-data\\zoo.xml";

    return (pricesFile, animalsFile, zooFile);
}

app.Run();
