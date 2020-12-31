//https://math.stackexchange.com/questions/2254655/hexagon-grid-coordinate-system
using System.IO;
using System.Collections.Generic;
var file = File.OpenRead("d24.txt");
//enum Color{ Black, White };
enum Direction{ NW, W, SW, NE, E, SE };
//for ease to use for part 2 <3
Dictionary<Direction, Coord> Offsets = new(){
        {Direction.NW, new Coord(0, -1)},
        {Direction.W, new Coord(-1, 0)},
        {Direction.SW, new Coord(-1, 1)},
        {Direction.NE, new Coord(1, -1)},
        {Direction.E, new Coord(1,0)},
        {Direction.SE, new Coord(0, 1)},
};
IEnumerable<Coord> AllOffsets = Offsets.Values;

HashSet<Coord> PosSet = new();
char c;
Coord CurrentPos = new Coord(0,0);

do{
    c = (char)file.ReadByte();
    switch(c){
        case 'e':
            Move(Direction.E);
            break;
        case 'n':
            c = (char)file.ReadByte();
            switch(c){
                case 'w':
                    Move(Direction.NW);
                    break;
                case 'e':
                    Move(Direction.NE);
                    break;
                default:
                    throw new ArgumentException("? after n");
            }
            break;
        case 's':
            c = (char)file.ReadByte();
            switch(c){
                case 'w':
                    Move(Direction.SW);
                    break;
                case 'e':
                    Move(Direction.SE);
                    break;
                default:
                    throw new ArgumentException("? after s");
            }
            break;
        case 'w':
            Move(Direction.W);
            break;
        case '\n':
        case (char)ushort.MaxValue:
            Filp(PosSet, CurrentPos);
            CurrentPos = new Coord(0,0);
            break;
    }
} while(c!=ushort.MaxValue); //EOF Char

void Move(Direction direction){
    var nextPos = GetPosition(CurrentPos, direction);
    CurrentPos = nextPos;
}
Coord GetPosition(Coord coord, Direction direction) =>
    coord + Offsets[direction];

void Filp(HashSet<Coord> set, Coord coord){
    if(!set.Add(coord)) {
        set.Remove(coord);
    }
}

struct Coord
{
    public int X {get;set;}
    public int Y {get;set;}
    public Coord(int x, int y){
        X = x;
        Y = y;
    }
    public static Coord operator +(Coord c1, Coord c2) => new Coord(c1.X+c2.X, c1.Y+c2.Y);
    public static Coord operator -(Coord c1, Coord c2) => new Coord(c1.X-c2.X, c1.Y-c2.Y);

    public override string ToString()
    {
        return $"[{X}, {Y}]";
    }
}
Console.WriteLine(PosSet.Count);

//-------------------Now 2
HashSet<Coord> FlipMany(HashSet<Coord> set)
{
    HashSet<Coord> mutableCoords = new HashSet<Coord>(set);
    HashSet<Coord> whiteBlocks = new();
    foreach(var coord in set)
    {
        switch(CountBlackTiles(coord, set)){
            case 1:
            case 2:
                break;
            default:
                mutableCoords.Remove(coord);
                break;
        }
        foreach(var block in GetAdjacentWhiteTiles(coord, set)){
            whiteBlocks.Add(block);
        }
    }
    foreach(var coord in whiteBlocks){
        if(CountBlackTiles(coord, set) == 2){
            mutableCoords.Add(coord);
        }
    }
    return mutableCoords;
}

IEnumerable<Coord> GetAdjacentWhiteTiles(Coord coord, HashSet<Coord> set) => AllOffsets.Select(item=>coord+item).Where(item=>!set.Contains(item));
int CountBlackTiles(Coord coord, HashSet<Coord> set) => AllOffsets.Count(item=>set.Contains(coord+item));

var __res = PosSet;
for(int i=0;i<100;i++){
    __res = FlipMany(__res);
}

Console.WriteLine(__res.Count);