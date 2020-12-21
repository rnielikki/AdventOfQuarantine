using System.IO;
using System.Collections.Generic;
using System.Text;

var reader = new StreamReader("d19.txt");
Dictionary<int, int[][]> rules = new();
Dictionary<int, string[]> memory = new();
string text = reader.ReadLine();


//not good parsing but it works.
do
{
    var t0 = text.Split(':');
    var index = int.Parse(t0[0]);
    if(t0[1][1]!='\"')
    {
        //or match
        var val = t0[1].Split('|').Select(t=>
            //and match
            t.Trim().Split().Select(val=>int.Parse(val)).ToArray()
        ).ToArray();
        rules.Add(index, val);
    }
    else
    {
        memory.Add(index, new string[]{t0[1][2].ToString()});
    }
    text = reader.ReadLine();
} while(text != "");

HashSet<string> messages = new();
do
{
    messages.Add(reader.ReadLine());
} while(!reader.EndOfStream);

//------------------- parsing end. start searching.
int max = messages.Max(msg=>msg.Length);
string[] findRec(int index)
{
    if(memory.ContainsKey(index))
    {
        return memory[index];
    }
    else if(rules.ContainsKey(index))
    {
        var rule = rules[index];
        var data = rule.Select(or=>
            or.Aggregate(new string[]{ "" }, (acc,item)=>
                getAnd(acc, findRec(item))
            )
        ).SelectMany(x=>x).ToArray();
        memory.Add(index, data);
        return data;
    }
    else
    {
        throw new System.ArgumentException("nope there are nothing on "+index);
    }
}
string[] getAnd(string[] first, string[] second){
    List<string> results = new();
    for(int i=0;i<first.Length;i++){
        for(int j=0;j<second.Length;j++){
            results.Add(first[i]+second[j]);
        }
    }
    return results.ToArray();
}
//The answer <3
Console.WriteLine(messages.Intersect(findRec(0)).Count());

//----------two.
var valids = messages.Where(asdf=>memory[42].Any(a=>asdf.StartsWith(a)) && memory[31].Any(b=>asdf.EndsWith(b)));
IEnumerable<(string, int)> GetStatus(string currentString, int memoryIndex, int requiredDepth = 1, int depth = 0){
    if(string.IsNullOrEmpty(currentString) || !memory[memoryIndex].Any(m=>currentString.StartsWith(m))){
        if(depth>=requiredDepth)
            return new (string, int)[]{ (currentString, depth) };
        else
            return Enumerable.Empty<(string, int)>();
    }
    return memory[memoryIndex].Where(i=> currentString.StartsWith(i)).SelectMany(crit=>
        GetStatus(currentString.Substring(crit.Length), memoryIndex, requiredDepth, depth+1)
    );
}
int rrr = 0;
foreach(var msg in valids){
    var res = GetStatus(msg, 42, 2).SelectMany(asdf=>
            GetStatus(asdf.Item1, 31, 1)
            .Where(st=>asdf.Item2 > st.Item2)
        ).Where(aaa=>aaa.Item1=="");
    rrr+=res.Count();
}
Console.WriteLine("I don't know "+rrr);