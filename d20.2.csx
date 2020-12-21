using System.IO;
using System.Linq;

var reader = new StreamReader("d20.txt");

Dictionary<int, bool[][]> data = new();
Dictionary<int, bool[][]> parsed = new();

while(!reader.EndOfStream){
    string txt = reader.ReadLine();
    if(txt!=""){
        if(txt[0]=='T'){
            int tileNumber = int.Parse(
                string.Concat(
                    txt.AsEnumerable()
                    .SkipWhile(c=>!char.IsDigit(c))
                    .TakeWhile(c=>char.IsDigit(c))
                )
            );
            txt = reader.ReadLine();
            List<bool[]> array = new();
            while(txt!="" && !reader.EndOfStream){
                array.Add(txt.Select(c=>c=='#'?true:false).ToArray());
                txt = reader.ReadLine();
            }
            if(txt!=""){
                array.Add(txt.Select(c=>c=='#'?true:false).ToArray());
            }

            var res = array.ToArray();
            data.Add(tileNumber, res);
            parsed.Add(
                tileNumber,
                GenerateSides(res)
            );
        }
    }
}

List<bool[]> values = parsed.SelectMany(p=>
        p.Value.Concat(
            p.Value.Select(
                v=>v.Reverse().ToArray()
            ).ToArray()
        )
    ).ToList();

//Console.WriteLine("one side of square is " + Math.Sqrt(parsed.Count));
var group = parsed.GroupBy(box=>
    box.Value.Count(line=>
        values.Count(v=>v.SequenceEqual(line)) <= 1
    )
).ToDictionary(kv => kv.Key, kv => kv.ToDictionary(x=>x.Key, x=>data[x.Key]));

KeyValuePair<int, bool[][]> fValue = group[2].Single(dt=>{
    for(int i=0;i<4;i++){
        //Console.WriteLine(oneSide==null);
        var sides = parsed[dt.Key];
        var a = group[1]
            .Select(kv=>FindMatch(kv.Value, sides[i], (byte)((i+2)%4) ))
            .Any(item=>item.Any());
            //Console.WriteLine(a+ " :: "+i);
        if(a){
            if(i==0 || i==3) return false;
        }
        else{
            if(i==1 || i==2) return false;
        }
    }
    return true;
});
//Console.WriteLine("the first::\n"+Visualize(fValue.Value));


Dictionary<Position, bool[][]> Image = new();
void MatchFromItems(Dictionary<int, bool[][]> findGroup, bool[][] values, int x, int y){
    var sides = GenerateSides(values);
    for(int i=0;i<4;i++){
        var a = findGroup.Select(kv=> (kv.Key, FindMatch(kv.Value, sides[i], (byte)((i+2)%4) ) )).Where(item=>item.Item2.Any());
        foreach(var asdf in a){
            var pos = Position.FromNumber(i,x,y);
            if(Image.ContainsKey(pos)) continue;
            data.Remove(asdf.Item1);
            Image.Add(pos, asdf.Item2);
            MatchFromItems(findGroup, asdf.Item2, pos.X, pos.Y);
        }
    }
}

bool[][] GenerateSides(bool[][] arr) => new bool[][]
    {
        arr[0], arr.Select(item=>item[^1]).ToArray(),
        arr[^1], arr.Select(item=>item[0]).ToArray()
    };

Image.Add(new Position{X=0,Y=0}, fValue.Value);
data.Remove(fValue.Key);
MatchFromItems(data, fValue.Value, 0, 0);
//Console.WriteLine(Image.Count);

int resultSize = (int)Math.Sqrt(parsed.Count);

for(int y=0;y<resultSize;y++){
    for(int l=1;l<9;l++){
        for(int x=0;x<resultSize;x++){
            var pos = new Position{X = x, Y = y};
            //var lenStart =  (x==0)?0:1;
            //var lenCut = (x==11)?0:1;
             //if(!((l==0 && y!=0) || (l==9 && y!=11)))
                Console.Write(VisualizeLine(Image[pos][l][1..^1]));
        }
        //if(!((l==0 && y!=0) || (l==9 && y!=11)))
        Console.WriteLine();
    }
}

/*
for(int i=0;i<4;i++){
    var a = group[1].Select(kv=> FindMatch(kv.Value, oneSide[i], (byte)((i+2)%4) )).Where(item=>item.Any());
    foreach(var asdf in a)
    Console.WriteLine($"{i}::\n{Visualize(asdf)}");
}

*/
string Visualize(bool[][] input){
    return string.Join("\n", input.Select(whatever=>VisualizeLine(whatever)) );
}
string VisualizeLine(bool[] input)=>
    string.Concat(input.Select(v=>v?'#':'.'));
/*
foreach(var box in parsed){

    //Console.WriteLine(box.Key+" :: "+v);
    if(v>=2){
        Console.WriteLine("2:: "+box.Key);
    }
    else if(v==1) {
        Console.WriteLine("1:: "+box.Key);
    }
};*/

bool[][] FindMatch(bool[][] piece, bool[] line, byte position){
    Func<bool[][], IEnumerable<bool>> getTarget = position switch{
        0=> (p) => p[0],
        1=> (p) => p.Select(x=>x[^1]),
        2=> (p) => p[^1],
        3=> (p) => p.Select(x=>x[0]),
        _=>throw new ArgumentException("wrong position "+position)
    };
    bool[][] currentStatus = piece;
    for(byte i=0;i<4;i++){
        var current = getTarget(currentStatus);
        if(current.SequenceEqual(line)) return currentStatus;
        else if(current.Reverse().SequenceEqual(line)){
            if(position%2 == 0){
                for(int x=0;x<10;x++){
                    currentStatus[x] = currentStatus[x].Reverse().ToArray();
                }
                return currentStatus;
            }
            else{
                for(int x=0;x<5;x++){
                    Swap(ref currentStatus[x], ref currentStatus[9-x]);
                }
                return currentStatus;
            }
        }
        if(i!=3) currentStatus = Rotate(currentStatus);
    }
    return new bool[][]{}; //no match
}

void Swap<T>(ref T a, ref T b)
{
    var temp= a;
    a = b;
    b = temp;
}

//I don't know how.
//https://www.geeksforgeeks.org/rotate-a-matrix-by-90-degree-in-clockwise-direction-without-using-any-extra-space/
bool[][] Rotate(bool[][] pieceRaw){
        //https://stackoverflow.com/questions/15725840/copy-one-2d-array-to-another-2d-array   
        bool[][] piece = pieceRaw.Select(item=>item.ToArray()).ToArray();
        for (int i = 0; i < 5; i++)
        {
            for (int j = i; j < 9 - i ; j++)
            {
                bool temp = piece[i][j];
                piece[i][j] = piece[9 - j][i];
                piece[9 - j][i] = piece[9 - i][9 - j];
                piece[9 - i][9 - j] = piece[j][9 - i];
                piece[j][9 - i] = temp;
            }
        }
        return piece;
}

struct Position {
    public int X {get; set;}
    public int Y {get; set;}
    public static Position FromNumber(int i, int x, int y){
        var pos = new Position();
        switch(i){
            case 0:
                pos.X = x;
                pos.Y = y-1;
                break;
            case 1:
                pos.X = x+1;
                pos.Y = y;
                break;
            case 2:
                pos.X = x;
                pos.Y = y+1;
                break;
            case 3:
                pos.X = x-1;
                pos.Y = y;
                break;
 
        }
        return pos;
    }
}