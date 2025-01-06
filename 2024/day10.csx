const string fileName = "day10.txt";
var data = File.ReadAllLines(fileName).Where(s => !string.IsNullOrWhiteSpace(s))
    .Select(a => a.ToCharArray().Where(c => c>=0x30 && c<0x3a).Select(c => (byte)(c - 0x30)).ToArray()).ToArray();
Coordinate[] allDirections = new Coordinate[4]{
    new Coordinate(-1, 0),
    new Coordinate(1, 0),
    new Coordinate(0, 1),
    new Coordinate(0, -1)
};

int trackResult = 0;
for(int y=0;y<data.Length;y++)
{
    for(int x=0;x<data[0].Length;x++)
    {
        if(data[y][x]==0)
        {
            trackResult+=TrackPath(data, new Coordinate(x,y), 0);
        }
    }
}
Console.WriteLine(trackResult);

int TrackPath(byte[][] data, Coordinate pos, int current)
{
    if(pos.IsInvalid(data)) return 0;
    else if(current==9)
    {
        return 1;
    }
    
    int yes = 0;
   foreach(var dir in allDirections)
    {
        var addedPos = pos + dir;
        var nextCurrent = current+1;
        if(!addedPos.IsInvalid(data) && data[addedPos.Y][addedPos.X] == nextCurrent)
        {
            var res = TrackPath(data, addedPos, nextCurrent);
            yes += res;
        } 
    }
   return yes;
}


struct Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
    public static Coordinate operator +(Coordinate coord1, Coordinate coord2) => new Coordinate(){ X = coord1.X + coord2.X, Y = coord1.Y + coord2.Y};
    public static Coordinate operator -(Coordinate coord1, Coordinate coord2) => new Coordinate(){ X = coord1.X - coord2.X, Y = coord1.Y - coord2.Y};
    public static Coordinate operator -(Coordinate coord) => new Coordinate(){ X = -coord.X, Y = -coord.Y};
    public bool Eq(Coordinate coord2) => X == coord2.X && Y == coord2.Y;
    public Coordinate(int x, int y) 
    {
        X=x;
        Y=y;
    }
    public override string ToString() => $"({X}, {Y})";
    public bool IsInvalid(byte[][] data) =>
        X<0 || Y<0 || X>=data[0].Length || Y>=data.Length;
}