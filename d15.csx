using System.IO;
using System.Linq;
using System.Collections.Generic;

var raw = File.ReadAllLines("d15.txt")[0].Split(',').Select(x=>int.Parse(x));
Dictionary<int, int> gameData = new();
int currentIndex = 1;

foreach(int r in raw){
    gameData.Add(r, currentIndex);
    currentIndex++;
}
int bufferNumber = 0;
int lastNumber = 0;
int Next()
{
    if(!gameData.ContainsKey(lastNumber))
    {
        lastNumber = 0;
    }
    else
    {
        lastNumber = currentIndex-gameData[lastNumber];
    }
    AddOrUpdate(bufferNumber, currentIndex);
    //Console.WriteLine($"added {bufferNumber} to {currentIndex}, lastNumber will be {lastNumber}");
    bufferNumber = lastNumber;
    
    currentIndex++;
    return 0;
}

void AddOrUpdate(int key, int value)
{
    if(gameData.ContainsKey(key)){
        gameData[key] = value;
    }
    else {
        gameData.Add(key, value);
    }
}

//const int round = 2020;
const int round = 30000000;

while(currentIndex<=round){
    Next();
}
Console.WriteLine(gameData.First(item=>item.Value==round));