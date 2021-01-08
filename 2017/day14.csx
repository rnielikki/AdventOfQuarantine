//const string code = "flqrgnkx"; //1242
const string code = "uugsqrei";

IEnumerable<int> tail = new int[]{17, 31, 73, 47, 23};

int SIZE = 256;

int[] CreateHash(IEnumerable<int> values, int[] arr, ref int cursor, ref int skipSize){
    foreach(int value in values){
        for(int index=0;index<value/2;index++){
            Swap(ref arr[(index+cursor)%SIZE], ref arr[(cursor+value-index-1)%SIZE]);
        }
        cursor = (cursor+value+skipSize)%SIZE;
        skipSize++;
    }
    return arr;
}
StringBuilder b = new StringBuilder();

for(int line=0;line<128;line++){
    var asciis = (code+"-"+line).ToCharArray().Select(item=>(int)item).Concat(tail);
    var arr = Enumerable.Range(0,SIZE).ToArray();
    int cursor = 0;
    int skipSize = 0;
    for(int i=0;i<64;i++){
        arr = CreateHash(asciis, arr, ref cursor, ref skipSize);
    }
    //Console.WriteLine(String.Join(',', arr));
    int arrIndex = 0;
    var c = arr.Take(16);
    while(c.Any()){
        b.Append(
            Convert.ToString(c.Aggregate((acc,val)=>acc^val),2).PadLeft(8,'0')
        );
        arrIndex+=16;
        c = arr.Skip(arrIndex).Take(16);
    }
}
Console.WriteLine(b.ToString().ToCharArray().Count(c=>c=='1'));


void Swap<T>(ref T x, ref T y){
    T temp = x;
    x = y;
    y = temp;
}

//------------2
Position.Init(b.ToString().ToCharArray());
Position.MakeRegions();
Console.WriteLine(Position.RegionCount);

struct Position{
    static HashSet<Position> _indexes = new HashSet<Position>();
    static List<HashSet<Position>> _regions = new();
    public static int RegionCount => _regions.Count;
    public int X {get;}
    public int Y {get;}
    public Position(int x, int y){
        X = x;
        Y = y;
    }
    public static void Init(char[] map){
        for(int i=0;i<128;i++){
            for(int j=0;j<128;j++){
                if(map[getPosIndex(i,j)]=='1'){
                    Position._indexes.Add(new Position(i,j));
                }
            }
        }
    }
    public IEnumerable<Position> GetAround()
    {
        var indexes = new Position[]{
            new Position(X-1, Y),
            new Position(X, Y-1),
            new Position(X+1, Y),
            new Position(X, Y+1)
        };
        return indexes.Where(ix=>_indexes.Contains(ix));
    }
    public static void MakeRegions()
    {
        foreach(var pos in _indexes){
            MakeRegion(pos);
        }
    }
    private static void MakeRegion(Position position)
    {
        var valids = position.GetAround();
        if( _regions.Any(item=>item.Contains(position))) return;
        if(!valids.Any()){
            _regions.Add(new HashSet<Position>(){position});
        }
        else{
            var region = valids.Select(pos=>
                _regions.SingleOrDefault(item=>item.Contains(pos))
            ).Where(x=>x!=null);

            if(!region.Any())
            {
                _regions.Add(new HashSet<Position>(){position});
            }
            else if(!region.Skip(1).Any())
            {
                region.Single().Add(position);
            }
            else{
                var newRegion = region.Aggregate(new List<Position>(), (acc, item) =>{ acc.AddRange(item); return acc; }).ToHashSet();
                newRegion.Add(position);
                foreach(var oldRegion in region){
                    _regions.Remove(oldRegion);
                }
                _regions.Add(newRegion);
            }
        }
    }
    private static int getPosIndex(int x, int y) => (x<0 || y<0 || x>128 || y>128)?-1:(x+y*128);
}