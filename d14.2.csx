using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

//Asserting first?
AssertSequence<long>(ChunckedParts(13), new long[]{8,4,1},nameof(ChunckedParts));
AssertSequence<long>(GetSumProbabilities(new long[]{3,9,14}), new long[]{3,12,9,26,23,17,14},nameof(GetSumProbabilities));
Assert(Convert.ToString(ConvertMask("10XX1001X"),2), "1100001", nameof(ConvertMask));
AssertSequence(GetAllPossibleBinaries(5), new long[]{0,1,4,5}, nameof(GetAllPossibleBinaries));

var reader = new StreamReader("d14.txt");

Dictionary<long, long> memories = new();
long currentMaskAnd = 0;
IEnumerable<long> currentMask = Enumerable.Empty<long>();
while(!reader.EndOfStream){
    var line = reader.ReadLine();
    var splitted = line.Split('=');
    var key = splitted[0].Trim();
    var value = splitted[1].Trim();
    if(key=="mask")
    {
        currentMaskAnd = Convert.ToInt64(value.Replace("X","1"), 2);
        currentMask = GetAllPossibleBinaries(ConvertMask(value));
    }
    else
    {
        //Well, VSCode doesn't recognize this, but this is part of C# 8 feature, should work!
        var memNumber = int.Parse(key[4..^1]);
        //Console.WriteLine("currentMemNumber" + memNumber);
        foreach(long mask in currentMask){
            long currentMemory = (currentMaskAnd | memNumber)-mask;
            //Console.WriteLine(Convert.ToString((memNumber),2)+" "+Convert.ToString((mask),2)+" :: "+Convert.ToString(currentValue,2));
            int theValue = int.Parse(value);
            if(memories.ContainsKey(currentMemory)){
                memories[currentMemory] = theValue;
            }
            else{
                memories.Add(currentMemory, theValue);
            }
        }
    }
}

//I can't stand without some unit tests
void Assert<T>(T expected, T value, string message){
    if(!expected.Equals(value)){
        throw new System.InvalidOperationException($"On {message}: expected {expected}, got {value}");
    }
}
void AssertSequence<T>(IEnumerable<T> expected, IEnumerable<T> value, string message){
    if(expected.Except(value).Any() || value.Except(expected).Any()){
        throw new System.InvalidOperationException($"On {message}: expected {expected} (length {expected.Count()}), got {value} (length {value.Count()})");
    }
}

long ConvertMask(string input)=>
    Convert.ToInt64(
        new StringBuilder(input).Replace('1','0').Replace('X','1').ToString()
        ,2
    );
IEnumerable<long> GetAllPossibleBinaries(long binaries) =>  GetSumProbabilities(ChunckedParts(binaries)).Append(0);
IEnumerable<long> ChunckedParts(long input){
    List<long> set = new();
    long current = 1;
    while(current<=input){
        long val = current&input;
        if(val!=0) set.Add(val);
        current<<=1;
    }
    return set;
}
IEnumerable<long> GetSumProbabilities(IEnumerable<long> input){
    if(!input.Any()) return Enumerable.Empty<long>();
    List<long> sums = new();
    long first = input.First();
    sums.Add(first);
    foreach(long val in GetSumProbabilities(input.Skip(1))){
        sums.Add(val+first);
        sums.Add(val);
    }
    return sums;
}

/*foreach(var val in memories){
    Console.WriteLine("**** "+val.Key+":: "+val.Value);
}*/
Console.WriteLine(checked(memories.Sum(kv=>kv.Value)));