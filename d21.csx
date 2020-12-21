using System.IO;
using System.Linq;
using System.Collections.Generic;

Dictionary<string, IEnumerable<string>> DataList = new();
List<string> Ingredients = new();
var data = new StreamReader("d21.txt");
string line = "";

while(!data.EndOfStream){
    line = data.ReadLine();
    var split0 = line.Split('(',2);
    var ingredients = split0[0].Split(' ')[0..^1]; //remove empty
    var allergic = split0[1][9..^1].Split(',').Select(ing=>ing.Trim());

    Ingredients.AddRange(ingredients);
    /*
    foreach(var f in ingredients){
        if(!Ingredients.Contains(f)){
            Ingredients.Add(f);
        }
    }
    */

    foreach(var alle in allergic){
        if(!DataList.ContainsKey(alle)){
            DataList.Add(alle, ingredients);
        }
        else{
            DataList[alle] = DataList[alle].Intersect(ingredients);
        }
    }
}
/*
foreach(var a in DataList){
    Console.Write(a.Key);
    foreach(var x in a.Value) Console.Write(" :: "+x);
    Console.WriteLine("");
}
*/
var nonAllergicFoods = Ingredients.Distinct().Except(
    DataList.Aggregate(DataList.First().Value, (agg, v)=>
        agg.Union(v.Value)
    )
);

var cnt = nonAllergicFoods.Sum(f=>
    Ingredients.Count(i=>i==f)
);
Console.WriteLine(cnt);

Dictionary<string, string> RealData = new();
List<string> Skipped = new();

foreach(var d in DataList.OrderBy(d=>d.Value.Count())){
    Console.WriteLine(d.Key+": " + d.Value.Count());
}

foreach(string key in DataList.OrderBy(d=>d.Value.Count()).Select(kv=>kv.Key)){
    var valueChecker = DataList[key];
    string value = "";
    if(valueChecker.Skip(1).Any()){
        value = DataList[key].Except(DataList.Where(kv=>kv.Key!=key).SelectMany(kv=>kv.Value)).SingleOrDefault();
        if(value == default){
            Console.WriteLine("skipping "+key+"...");
            Skipped.Add(key);
            continue;
        }
    }
    else{
        value = valueChecker.Single();
    }
    Console.WriteLine(key);
    RealData.Add(key, value);
    foreach(var d in DataList){
        DataList[d.Key] = DataList[d.Key].Where(v=>v!=value);
    }
    //string value = kvpair.Value.Except(DataList.Where(kv=>kv.Key!=key).SelectMany(kv=>kv.Value)).Single();
    //RealData.Add(key, value);
}

foreach(string skippedData in Skipped){
    RealData.Add(skippedData, DataList[skippedData].Single());
    //DataList[skippedData].Except(RealData.Select(kv=>kv.Value));
}

Console.WriteLine(string.Join(",", RealData.OrderBy(kv=>kv.Key).Select(kv=>kv.Value)));

/*
foreach(var non in nonAllergicFoods){
    Ingredients.Count(i=>i == non);
}*/

/*
var data = File.ReadAllLines("d21.txt")
    .Select(item=>{
        var split0 = item.Split('(',2);
        var item0 = split0[0].Split(' ')[0..^1]; //remove empty
        var item1 = split0[1][9..^1].Split(',').Select(ing=>ing.Trim());

        if(item0 =)

        return (item0, item1);
    });
*/

/*
foreach(var d in data){
    foreach(var asdf in d.Item1){
        Console.WriteLine("::"+asdf);
    }
    foreach(var aaa in d.Item2){
        Console.WriteLine(" contains "+aaa);
    }
}
*/