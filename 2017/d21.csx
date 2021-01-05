IEnumerable<bool> maps
 = File.ReadAllLines("d21.begin.txt").Select(item=>item.ToCharArray().Select(item=>item=='#'?true:false)).SelectMany(x=>x);

var txt =new StreamReader("d21.txt");
while(!txt.EndOfStream){
    string line = txt.ReadLine();
    Rule.RulesList.Add(new Rule(line));
}

//bool[][] current
//= File.ReadAllLines("d21.begin.txt").Select(item=>item.ToCharArray().Select(item=>item=='#'?true:false).ToArray()).ToArray();
//int length = (int)Math.Sqrt(maps.Count());
int iter = 0;
int MAX_ITER = 18;


do{
    if(iter<1){
        maps = Rule.Find(maps);
        Map.Length++;
    }
    else{
        maps = Map.Cut(maps);
    }
    iter++;
    Console.WriteLine("generation"+iter+": "+maps.Sum(x=>x?1:0));//assure
} while(iter<MAX_ITER);
Console.WriteLine(maps.Sum(x=>x?1:0));
// 2100000 < < 2309840

class Rule
{
    public static List<Rule> RulesList = new();
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
    public static IEnumerable<bool> Find (IEnumerable<bool> Value) => RulesList.Single(item=>item.FindMatch(Value)).Value;
    private bool FindMatch(IEnumerable<bool> Value) => matches.Any(item=>item.SequenceEqual(Value));
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
    public IEnumerable<bool> Value;
    public static int Length { get; set;} = 3;
    public Map(IEnumerable<bool> value){
        Value = value;
        //Size = (int)Math.Sqrt(Value.Count());
    }
    public static IEnumerable<bool> Cut(IEnumerable<bool> map){
        int chunkLength = 2+Length%2;
        List<bool> mapList = new();
        //int length = (int)Math.Sqrt(map.Count());
        int length = Length;
        int amount = length/chunkLength;
        for(int l=0;l<amount;l++){
            List<IEnumerable<bool>> blockCollection = new();
            for(int i =0;i<amount;i++){
                bool[][] block = new bool[chunkLength][];
                for(int j=0;j<chunkLength;j++){
                    block[j] = 
                        map
                        .Skip(
                            chunkLength*i
                            +length*j
                            +chunkLength*length*l)
                        .Take(chunkLength).ToArray();
                }
                blockCollection.Add(
                    Rule.Find(block.SelectMany(x=>x))
                );
            }
            for(int x=0;x<=amount;x++){
                /*
                Console.WriteLine(
                    String.Join("\n", blockCollection.Select
                        (b=>
                            new String(b.Select(asdf=>asdf?'#':'.').ToArray())
                        )
                    )
                );*/
                mapList.AddRange(blockCollection.SelectMany(item=>item.Skip(x*(chunkLength+1)).Take(chunkLength+1)));
            }
            //Console.WriteLine("---");
            blockCollection.Clear();
        }
        //Length=(int)length*chunkLength;
        Length+=length/(2+length%2);
        //Console.WriteLine($"chunkLength: {chunkLength} / Length: {length} / List Size : {mapList.Count}");

        //Console.WriteLine(new String(mapList.Select(asdf=>asdf?'#':'.').ToArray()));
        return mapList;
    }
    public override string ToString()
    {
        return new String(Value.Select(b=>b?'#':'.').ToArray());
    }
}