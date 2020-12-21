using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

var reader = new StreamReader("d16.txt");


//others

//-----------------------------------------------------
string line;
List<int[]> ranges = new();
do {
    line = reader.ReadLine();
    if(line is {Length:>0})
    {
        ranges.Add(ParseLine(line));
    }
}while(line!="");

List<int[]> tickets = new();
reader.ReadLine(); // your ticket text
int[] mine = reader.ReadLine().ParseTicket();
tickets.Add(mine);

reader.ReadLine(); //Empty Line
reader.ReadLine(); //NEarby Tickets line

int nonValids = 0;
do {
    int[] ticket = reader.ReadLine().ParseTicket();
    var invalids = ticket.Where(item=>
            !ranges.Any(range=> ValidateLine(range, item))
    );
    if(!invalids.Any())
    {
        tickets.Add(ticket);
    }
    else
    {
        nonValids+=invalids.Sum();
    }
}while(!reader.EndOfStream);
Console.WriteLine("first: "+nonValids);

//------------------------------- now we know what's valid.
//Love so much Linq!
List<(int, int)> remapper = new();
Dictionary<int, int[]> remapped = new();
for(int rangeIndex=0;rangeIndex<ranges.Count;rangeIndex++){
    var range = ranges[rangeIndex];
    for(int i=0;i<ranges.Count;i++){
        if(tickets.Select(t=>t[i]).All(t=>ValidateLine(range,t))){
            remapper.Add((i, rangeIndex)); //Item2 is rangeIndex
        }
    }
}
var groupped = remapper.GroupBy(item=>item.Item1).OrderBy(item=>item.Count());
Dictionary<int,int> map = new();

foreach(var kv in groupped){
    foreach(var indexes in kv){
        if(!map.ContainsKey(indexes.Item2))
        {
            map.Add(indexes.Item2, indexes.Item1);
            //Console.WriteLine($"mapped {indexes.Item2} to {indexes.Item1}");
            break;
        }
    }
}
 //Item2, so key is rangeIndex. Sorry but not sorry for bad code, it's mine!
 Console.WriteLine(mine[0]);
 Console.WriteLine(mine[1]);
 Console.WriteLine(mine[2]);
var maybeResult = map.OrderBy(dic=>dic.Key).ToList().Select(item=>item.Value).Take(6).Aggregate(1L, (acc, val)=>checked(acc*mine[val]));

Console.WriteLine(maybeResult);

static int[] ParseTicket (this string input) => input.Split(',').Select(item=>int.Parse(item)).ToArray();
bool ValidateLine(int[] rangeData, int data) => ((rangeData[0] <= data && rangeData[1] >= data)
            ||
            (rangeData[2] <= data && rangeData[3] >= data));

//This means I'm done with parsing, don't even bother me.

static Regex rule = new Regex(@"\d+", RegexOptions.Compiled | RegexOptions.Singleline);
int[] ParseLine(string targetLine)
{
    return rule.Matches(targetLine).Select(m=>int.Parse(m.Value)).ToArray();
}