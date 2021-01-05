using System.IO;
using System.Collections.Generic;

enum Status{
     Infected,
     Weakened,
     Flagged
}
HashSet<Position> Viruses = new();

var indexes = File.ReadAllLines("day22.txt").Select(item=>item.ToCharArray()).ToArray();
Init();

int MAX_ACTION = 10000;
int InfectoinActivity = 0;
for(int i=0;i<MAX_ACTION;i++)
{
    if(Viruses.Contains(Cursor.Position))
    {
        Cursor.Rotate(1);
        Viruses.Remove(Cursor.Position);
    }
    else
    {
        Cursor.Rotate(-1);
        Viruses.Add(new Position(Cursor.Position.X, Cursor.Position.Y));
        InfectoinActivity++;
    }
    Cursor.Move();
}
Console.WriteLine(InfectoinActivity);

Init();
MAX_ACTION = 10000000;
InfectoinActivity = 0;

for(int i=0;i<MAX_ACTION;i++)
{
    if(Viruses.TryGetValue(Cursor.Position, out Position pos))
    {
        switch(pos.Status)
        {
            case Status.Weakened:
                pos.Status = Status.Infected;
                InfectoinActivity++;
                break;
            case Status.Infected:
                Cursor.Rotate(1);
                pos.Status = Status.Flagged;
                break;
            case Status.Flagged:
                Cursor.Rotate(2);
                Viruses.Remove(pos);
                break;
        }
    }
    else
    {
        Cursor.Rotate(-1);
        Viruses.Add(new Position(Cursor.Position.X, Cursor.Position.Y, Status.Weakened));
    }
    Cursor.Move();
}
Console.WriteLine(InfectoinActivity);

void Init(){
    Viruses.Clear();
    for(int i=0;i<indexes.Length;i++){
        for(int j=0;j<indexes[0].Length;j++){
            if(indexes[i][j]=='#')
            {
                Viruses.Add(new Position(j,i));
            }
        }
    }
    Cursor.Init(indexes[0].Length/2, indexes.Length/2);
}


class Position
{
    public Status Status { get; set;}
    public int X { get; }
    public int Y { get; }
    
    public Position(int x, int y):this(x, y, Status.Infected){}
    public Position(int x, int y, Status status){
        X = x;
        Y = y;
        Status = status;
    }
    public static Position operator + (Position pos1, Position pos2) => new Position(pos1.X+pos2.X, pos1.Y+pos2.Y);
    public static Position operator - (Position pos1, Position pos2) => new Position(pos1.X-pos2.X, pos1.Y-pos2.Y);
    public override bool Equals(object obj)
    {   
        if (obj is Position pos)
        {
            return pos.X == X && pos.Y == Y;
        }
        return false;
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        return X*99999+Y;
    }
}

static class Cursor {
    public static Position Position { get; private set; }
    private static Position[] directions = new[]{
        new Position(0,-1),
        new Position(1, 0),
        new Position(0, 1),
        new Position(-1, 0)
    };
    private static int _dirIndex = 0;
    private static Position _direction => directions[_dirIndex];
    public static void Move()
    {
        Position += _direction;
    }
    public static void Init(int x, int y){
        Position = new Position(x, y);
        _dirIndex = 0;
    }
    public static void Rotate(int scale)
    {
        _dirIndex=(_dirIndex+scale+4)%4;
    }
}