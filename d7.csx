using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

ConcurrentDictionary<string, IEnumerable<(string, int)>> bagSet = new();

var dataStream = File.OpenRead("d7.txt");
var textReader = new StreamReader(dataStream);
string line = textReader.ReadLine();
do {
    var bagData = line.Split(" contain ");
    IEnumerable<(string, int)> items;
    if (bagData[1] != "no other bags.")
    {
        items = bagData[1][0..^1].Split(", ").Select(
            item =>
            {
                var parts = item.Split(' ', 2);
                var count = int.Parse(parts[0]);
                return (parts[1]+(count==1?"s":""), count);
            }
        );
    }
    else
    {
        items = new List<(string, int)>();
    }
    bagSet.TryAdd(bagData[0], items);
    line = textReader.ReadLine();
} while (line != null);

//1: lol remove itself
/*
Console.WriteLine(
    (await Task.WhenAll(bagSet.Keys.Select(item => FindShinyGold(item))) ).Where(condition => condition).Count() -1
);

async Task<bool> FindShinyGold(string bag)
{
    if (bag == "shiny gold bags") return true;
    if (bagSet.TryGetValue(bag, out IEnumerable<(string, int)> containingBags))
    {
        if (!containingBags.Any()) return false;
        else
        {
            return (await Task.WhenAll(containingBags.Select(bag => FindShinyGold(bag.Item1)))).Any(val=>val==true);
        }
    }
    else return false;
}
*/

//2: 
Console.WriteLine(
    await CheckBags("shiny gold bags", 1) - 1
);

async Task<int> CheckBags(string bag, int count)
{
    if (bagSet.TryGetValue(bag, out IEnumerable<(string, int)> containingBags))
    {
        if (!containingBags.Any()) return count;
        else
        {
            return ((
                    await Task.WhenAll(
                        containingBags.Select(val => CheckBags(val.Item1, val.Item2))
                    )
                )
                .Sum()+1) * count;
        }
    }
    else
    {
        throw new KeyNotFoundException($"what is the {bag}? shouldn't be");
    }
}
