using System.Xml.Linq;
using ZooFoodCost.Domain;

namespace ZooFoodCost.Infrastructure;

public class ZooXmlParser : IZooXmlParser
{
    public IEnumerable<ZooAnimal> Parse(string filePath, IEnumerable<AnimalSpecies> species)
    {
        var doc = XDocument.Load(filePath);
        var root = doc.Element("Zoo");
        if (root is null)
            throw new InvalidOperationException("Zoo element not found in XML");

        var speciesLookup = species.ToDictionary(s => s.Name.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);

        foreach (var animalContainer in root.Elements())
        {
            foreach (var animalElement in animalContainer.Elements())
            {
                var speciesName = animalElement.Name.LocalName;
                if (!speciesLookup.TryGetValue(speciesName.ToLowerInvariant(), out var matchedSpecies))
                {
                    throw new ZooFoodCostException($"Unknown animal species '{speciesName}' found in Zoo data filecls. Please add species data to Animals data file.");
                }

                var name = animalElement.Attribute("name")!.Value;
                var kgString = animalElement.Attribute("kg")!.Value;
                var weight = decimal.Parse(kgString);

                yield return new ZooAnimal(name, weight, matchedSpecies);
            }
        }
    }
}
