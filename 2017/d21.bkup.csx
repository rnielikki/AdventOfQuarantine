List<Rule> rules = new List<Rule>();
List<Map> maps = new List<Map>();

var txt =new StreamReader("d21.txt");
while(!txt.EndOfStream){
    string line = txt.ReadLine();
    rules.Add(new Rule(line));
}

//bool[][] current
//= File.ReadAllLines("d21.begin.txt").Select(item=>item.ToCharArray().Select(item=>item=='#'?true:false).ToArray()).ToArray();
var initialMap = File.ReadAllLines("d21.begin.txt").Select(item=>item.ToCharArray().Select(item=>item=='#'?true:false)).SelectMany(x=>x);
maps.Add(new Map(initialMap));
int length = (int)Math.Sqrt(initialMap.Count());
int iter = 0;
int MAX_ITER = 18;


do{
    Console.WriteLine(iter);
    if(length>3){
        maps = Map.Cut(
            maps.Aggregate(Enumerable.Empty<bool>(), (acc,item)=>acc.Concat(item.Value)),
        2+length%2);
        //Console.WriteLine(maps.Count+", with "+maps.First());
        length=(int)(Math.Sqrt(maps.Count))*maps.First().Size;
    }
    else{
        length++;
    }
    foreach(var map in maps){
        map.Value = rules.Single(item=>item.FindMatch(map.Value)).Value;
        //Console.WriteLine(map.ToString());
    }
    iter++;
} while(iter<MAX_ITER);
Console.WriteLine(maps.Sum(item=>item.Value.Where(x=>x).Count()));

class Rule
{
    private readonly List<IEnumerable<bool>> matches = new();
    public IEnumerable<bool> Value { get; }
    public int Dimension { get; }
    public Rule(string rule)
    {
        string[] split1 = rule.Split('=');
        var toMatch = StringToArray(split1[0].Trim());
        Dimension = toMatch.Length;
        AddAllMaps(toMatch);
        Value = split1[1].Substring(2).Trim().ToCharArray().Where(item=>item!='/').Select(item => item switch {
            '.' => false,
            '#' => true,
            _ => throw new ArgumentException("wrong char found: "+item)
        });
    }
    public bool FindMatch(IEnumerable<bool> Value) => matches.Any(item=>item.SequenceEqual(Value));
    private bool[][] StringToArray(string str)
        => str.Split('/').Select(item=>item.ToCharArray().Select(c=>c=='#'?true:false).ToArray()).ToArray();
        
    //flip. rotate.
    private void AddAllMaps(bool[][] Value)
    {
        bool[][] current = Value;
        bool horizontal = true;
        for(int i=0;i<4;i++)
        {
            matches.Add(current.SelectMany(x=>x));
            matches.Add(Filp(current, horizontal).SelectMany(x=>x));
            current = Rotate(current);
            horizontal = !horizontal;
        }
    }
    //I still don't have idea how to rotate!
    //https://stackoverflow.com/questions/42519/how-do-you-rotate-a-two-dimensional-array
    private bool[][] Rotate(bool[][] matrix) {
        bool[][] ret = new bool[Dimension][].Select(_=>new bool[Dimension]).ToArray();
        for (int i = 0; i < Dimension; ++i) {
                for (int j = 0; j < Dimension; ++j) {
                    ret[i][j] = matrix[Dimension - j - 1][i];
                }
            }
        return ret;
    }
    private bool[][] Filp(bool[][] Value, bool horizontal)
    {
        if(horizontal)
        {
            return Value.Select(item=>item.Reverse().ToArray()).ToArray();
        }
        else
        {
            return Value.Reverse().ToArray();
        }
    }
}
class Map {
    private IEnumerable<bool> _Value;
    public IEnumerable<bool> Value { get => _Value; set { _Value = value; Size++; } }
    public int Size { get; private set; }
    public Map(IEnumerable<bool> value){
        Value = value;
        Size = (int)Math.Sqrt(Value.Count());
    }
    public static List<Map> Cut(IEnumerable<bool> map, int chunkLength){
        List<Map> mapList = new();
        int length = (int)Math.Sqrt(map.Count());
        int amount = length/chunkLength;
        for(int l=0;l<amount;l++){
            for(int i =0;i<amount;i++){
                List<bool> block = new();
                for(int j=0;j<chunkLength;j++){
                    block.AddRange(map.Skip(chunkLength*i+chunkLength*chunkLength*j+l*chunkLength*chunkLength*amount).Take(chunkLength));
                }
                mapList.Add(new Map(block));
            }
        }
        return mapList;
    }
    public override string ToString()
    {
        return new String(Value.Select(b=>b?'#':'.').ToArray());
    }
}