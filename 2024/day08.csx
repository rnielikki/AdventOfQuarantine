int lineNumner = 0;
char[][] data = File.ReadAllLines("day08.txt").Select(c => c.ToCharArray()).ToArray();
Coordination dataLen = new Coordination(){ X = data[0].Length, Y = data.Length};
Dictionary<char, List<AlphabetInfo>> allAlphabets = new();
//generate alphabet info
for(int y=0;y<data.Length;y++)
{
    for(int x=0;x<data[0].Length;x++)
    {
        char c = data[y][x];
        if(c=='.') continue;
        var info = new AlphabetInfo(){
                Alphabet = c,
                Coord = new Coordination()
                {
                    X = x,
                    Y = y
                }
        };
        if(!allAlphabets.ContainsKey(c)) allAlphabets.Add(c, new List<AlphabetInfo>());
        allAlphabets[c].Add(info);
    }
}

HashSet<Coordination> results = new();

foreach(var alphabetInfo in allAlphabets)
{
    var c = alphabetInfo.Key;
    SetUniqueLocations(alphabetInfo.Value.ToArray());
}

void SetUniqueLocations(AlphabetInfo[] infoCol)
{
    for(int i=0;i<infoCol.Length;i++)
    {
        for(int j = 0; j < infoCol.Length ;j++)
        {
            if(j==i) continue;
            var posDiff = infoCol[i].Coord - infoCol[j].Coord;
            var position = infoCol[i].Coord + posDiff;
            while(position.IsValid(dataLen))
            {
                if(!results.Contains(position))
                {
                    results.Add(position);
                }

                position += posDiff;
            }
            position = infoCol[i].Coord;
            while(position.IsValid(dataLen))
            {
                if(!results.Contains(position))
                {
                    results.Add(position);
                }

                position -= posDiff;
            }
        }
    }
}
Console.WriteLine(results.Count);

public struct AlphabetInfo
{
    public char Alphabet { get; set; }
    public Coordination Coord { get; set; }
}

public struct Coordination
{
    public int X { get; set; }
    public int Y { get; set; }
    public static Coordination operator +(Coordination coord1, Coordination coord2) => new Coordination(){ X = coord1.X + coord2.X, Y = coord1.Y + coord2.Y};
    public static Coordination operator -(Coordination coord1, Coordination coord2) => new Coordination(){ X = coord1.X - coord2.X, Y = coord1.Y - coord2.Y};
    public static Coordination operator -(Coordination coord) => new Coordination(){ X = -coord.X, Y = -coord.Y};
    public override string ToString() => $"({X}, {Y})";
    public bool IsValid(Coordination reference) => X > -1 && Y > -1 && X < reference.X && Y < reference.Y;
}