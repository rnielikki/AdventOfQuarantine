var raw = File.ReadAllLines("day19.txt");
//-------- sample
var sample = raw[0].Split(", ");
//------- data
var data = raw.Skip(2).Where(a=>!string.IsNullOrWhiteSpace(a)).ToArray();
Dictionary<string,long> memo = new();
//------------ end of the input ----------//

string s = "";
long count;

foreach(var asdf in data)
{
    count+=SearchEach(asdf, sample);
}
Console.WriteLine(count);

long SearchEach(string s, string[] arr)
{
    if (s.Length == 0) return 1;
    if (memo.TryGetValue(s, out long count))
    {
        return count;
    }
    foreach (string arrPart in arr)
    {
        //if valid both...
        if (s.StartsWith(arrPart))
        {
            count+=SearchEach(s.Substring(arrPart.Length), arr);
        }
    }
    memo.Add(s, count);
    return count;
}