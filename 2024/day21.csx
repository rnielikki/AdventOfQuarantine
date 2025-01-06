#load  "./_libcoord.csx"
var data = File.ReadAllLines("day21.txt");
Coordinate[] padMap =
[
    new Coordinate(1,3), //0
    new Coordinate(0,2), //1
    new Coordinate(1,2), //2
    new Coordinate(2,2), //3
    new Coordinate(0,1), //4
    new Coordinate(1,1), //5
    new Coordinate(2,1), //6
    new Coordinate(0,0), //7
    new Coordinate(1,0), //8
    new Coordinate(2,0), //9
    new Coordinate(2,3), //A
];

//key = actual direction, value: pad button coordinate
Dictionary<DirectionType, Coordinate> arrowPadMap = new()
{
    {DirectionType.None, new Coordinate(2,0)},
    {DirectionType.Up, new Coordinate(1,0)},
    {DirectionType.Down, new Coordinate(1,1)},
    {DirectionType.Left, new Coordinate(0,1)},
    {DirectionType.Right, new Coordinate(2,1)},
};

Coordinate emptyPad = new Coordinate(0, 3);
Coordinate emptyArrowPad = new Coordinate(0, 0);
Coordinate pressArrowPad = new Coordinate(2, 2);

Dictionary<char, Dictionary<char, Coordinate>> movementMap =
    new() { { 'A', new Dictionary<char, Coordinate>() } };

//directiontype none is A button
Dictionary<string, long> saver = new();
Dictionary<DirectionType, Dictionary<DirectionType, Coordinate>> movementArrowMap = new();

long final = 0;
foreach (var l in data)
{
    final+=RunLine(l);
}
Console.WriteLine(final);
long RunLine(string input)
{
    var testResult = new List<DirectionType>();
    var currentPos = padMap[10];
    foreach (char c in input)
    {
        var nextPos = GetCoordFromChar(c);
        var r =ReadNumPad(currentPos, nextPos, emptyPad);
        testResult.AddRange(r);
        testResult.Add(DirectionType.None);
        currentPos = nextPos;
    }
    var temp = new List<DirectionType>();
    return WhatIsThis(testResult, 24)*int.Parse(input.Substring(0,3));
}
List<List<DirectionType>> Whatever(List<DirectionType> seq)
{
    List<List<DirectionType>> temp = new();
    var currentPos = arrowPadMap[DirectionType.None];
    foreach(var nextDir in seq)
    {
        var nextPos = arrowPadMap[nextDir];
        var r =ReadArrowPad(currentPos, nextPos, emptyArrowPad);
        r.Add(DirectionType.None);
        temp.Add(r);
        currentPos = nextPos;
    }
    return temp;
}
long WhatIsThis(List<DirectionType> seq, int depth)
{
    var key = GetKey(seq, depth);
    if(saver.TryGetValue(key, out long res)) return res;
    var asdf = Whatever(seq);
    if(depth==0) return asdf.SelectMany(a=>a).Count();
    long sum = 0;
    foreach(var part in asdf)
    {
        sum+=WhatIsThis(part, depth-1);
    }
    saver.Add(key, sum);
    return sum;
}
string GetKey(List<DirectionType> seq, int depth) => string.Join("", seq.Select(TypeToChar)) + depth.ToString();
IEnumerable<DirectionType> ReadNumPad(Coordinate start, Coordinate end, Coordinate empty)
{
    var distance = end - start;
    if (distance.X == 0 && distance.Y == 0) return Enumerable.Empty<DirectionType>();
    List<DirectionType> dirs = new();
    if (distance.X < 0 && end.X == 0 && start.Y == empty.Y)
    {
        AddY(); AddX();
    }
    else if(distance.X < 0)
    {
        AddX(); AddY();
    }
    else if(end.Y == empty.Y && start.X==0)
    {
        AddX(); AddY();
    }

    else
    {
        AddY(); AddX();
    }
    return dirs;

    void AddX()
    {
        if (distance.X != 0) dirs.AddRange(Enumerable.Repeat((distance.X < 0) ? DirectionType.Left : DirectionType.Right, Math.Abs(distance.X)));
    }
    void AddY()
    {
        if (distance.Y != 0) dirs.AddRange(Enumerable.Repeat((distance.Y < 0) ? DirectionType.Up : DirectionType.Down, Math.Abs(distance.Y)));
    }
}
List<DirectionType> ReadArrowPad(Coordinate start, Coordinate end, Coordinate empty)
{
    var distance = end - start;
    if (distance.X == 0 && distance.Y == 0) return new List<DirectionType>();

    List<DirectionType> dirs = new();

    if(distance.X < 0) {
        if(end.X==0 && start.Y==empty.Y){ AddY(); AddX(); }
        else { AddX(); AddY(); }
    }
    else if(end.Y==empty.Y && start.X == 0)
    {
        AddX(); AddY();
    }
    else {
        AddY(); AddX();
    }
    return dirs;

    void AddX()
    {
        if (distance.X != 0) dirs.AddRange(Enumerable.Repeat((distance.X < 0) ? DirectionType.Left : DirectionType.Right, Math.Abs(distance.X)));
    }
    void AddY()
    {
        if (distance.Y != 0) dirs.AddRange(Enumerable.Repeat((distance.Y < 0) ? DirectionType.Up : DirectionType.Down, Math.Abs(distance.Y)));
    }
}


Coordinate GetFirstPadPress(char start, char end, Coordinate empty)
{
    return GetPadPress(GetCoordFromChar(start), GetCoordFromChar(end));
}
Coordinate GetPadPress(Coordinate startPoint, Coordinate destination) => destination - startPoint;

Coordinate GetCoordFromChar(char c)
{
    var num = (int)(c - 0x30);
    //A button
    var destination = padMap[10];
    if (num < 10) destination = padMap[num];
    return destination;
}

// DEBUG 
char TypeToChar(DirectionType dType) => dType switch
{
    DirectionType.None => 'A',
    DirectionType.Up => '^',
    DirectionType.Left => '<',
    DirectionType.Right => '>',
    DirectionType.Down => 'v',
    _ => throw new NotImplementedException()
};