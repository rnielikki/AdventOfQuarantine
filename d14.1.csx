using System.IO;
using System.Text;
using System.Collections.Generic;

var reader = new StreamReader("d14.txt");

Dictionary<int, long> memories = new();
long currentMaskAnd = 0;
long currentMaskOr = 0;
while(!reader.EndOfStream){
    var line = reader.ReadLine();
    var splitted = line.Split('=');
    var key = splitted[0].Trim();
    var value = splitted[1].Trim();
    if(key=="mask")
    {
        currentMaskAnd = Convert.ToInt64(value.Replace("X","1"), 2);
        currentMaskOr = Convert.ToInt64(value.Replace("X","0"), 2);
    }
    else
    {
        //Well, VSCode doesn't recognize this, but this is part of C# 8 feature, should work!
        var memNumber = int.Parse(key[4..^1]);
        //Console.WriteLine("currentMemNumber" + memNumber);
        long currentValue = checked(currentMaskAnd & int.Parse(value)) | currentMaskOr;
        if((memories.ContainsKey(memNumber))){
            memories[memNumber] = currentValue;
        }
        else{
            memories.Add(memNumber, currentValue);
        }
        Console.WriteLine(currentValue);
    }
}
foreach(var val in memories){
    Console.WriteLine("**** "+val.Key+":: "+val.Value);
}
Console.WriteLine(checked(memories.Sum(kv=>kv.Value)));