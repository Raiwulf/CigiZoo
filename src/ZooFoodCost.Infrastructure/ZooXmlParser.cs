using System.Xml.Linq;
using ZooFoodCost.Domain;

namespace ZooFoodCost.Infrastructure;

public class ZooXmlParser : IZooXmlParser
{
    public IEnumerable<ZooAnimal> Parse(string filePath, IEnumerable<AnimalSpecies> species)
    {
        var doc = XDocument.Load(filePath);
        var root = doc.Element("Zoo");
        var speciesLookup = species.ToDictionary(s => s.Name.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);

        foreach (var animalContainer in root.Elements())
        {
            var containerName = animalContainer.Name.LocalName;
            var speciesName = ToSingular(containerName);
            var matchedSpecies = speciesLookup[speciesName.ToLowerInvariant()];

            foreach (var animalElement in animalContainer.Elements())
            {
                var name = animalElement.Attribute("name").Value;
                var kgString = animalElement.Attribute("kg").Value;
                var weight = decimal.Parse(kgString);

                yield return new ZooAnimal(name, weight, matchedSpecies);
            }
        }
    }

    private static string ToSingular(string plural)
    {
        var irregularPlurals = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Lions", "Lion" },
            { "Tigers", "Tiger" },
            { "Giraffes", "Giraffe" },
            { "Zebras", "Zebra" },
            { "Wolves", "Wolf" },
            { "Piranhas", "Piranha" }
        };

        if (irregularPlurals.TryGetValue(plural, out var singular))
            return singular;

        if (plural.EndsWith("ies", StringComparison.OrdinalIgnoreCase))
            return plural[..^3] + "y";
        if (plural.EndsWith("es", StringComparison.OrdinalIgnoreCase))
            return plural[..^2];
        if (plural.EndsWith("s", StringComparison.OrdinalIgnoreCase) && plural.Length > 1)
            return plural[..^1];

        return plural;
    }
}
