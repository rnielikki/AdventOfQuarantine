using System;
using System.IO;

struct Point {
    public int X { get; set;}
    public int Y { get; set; }
    public Point(int x, int y){
        X = x; Y = y;
    }
}

class ShipStatus {
    char[] directions = new char[]{'N', 'E', 'S', 'W'};
    public char Direction { get; set; }
    private Point _point;
    public ShipStatus(char direction, int x, int y)
    {
        Direction = direction;
        _point = new Point(x,y);
    }
    public ShipStatus SetDirection(int scale)
    {
        int index = Array.IndexOf(directions, Direction);
        Direction = directions[(index+scale+4)%4];
        return this;
    }
    public ShipStatus MoveX(int x)
    {
        _point.X += x;
        return this;
    }
    public ShipStatus MoveY(int y)
    {
        _point.Y += y;
        return this;
    }
    public override string ToString()
    {
        return $"To the {Direction} : {_point.X}, {_point.Y}";
    }
    public int GetResult() =>
        Math.Abs(_point.X)+Math.Abs(_point.Y);
  }

ShipStatus ChangeStatus(char direction, int scale, ShipStatus status) => direction switch {
    'E' => status.MoveX(1*scale),
    'W' => status.MoveX(-1*scale),
    'N' => status.MoveY(1*scale),
    'S' => status.MoveY(-1*scale),
    'F' => ChangeStatus(status.Direction, scale, status),
    'L' => status.SetDirection(-scale/90),
    'R' => status.SetDirection(scale/90),
    _ => throw new ArgumentException()
};

var str = new StreamReader("d12.txt");
ShipStatus status = new ShipStatus('E', 0, 0);
while(!str.EndOfStream){
    (char c, int i) = Parse(str.ReadLine());
    ChangeStatus(c, i, status);
    //Console.WriteLine(status.ToString());
}
Console.WriteLine(status.GetResult());
(char, int) Parse(string str) => (str[0], int.Parse(str.Substring(1)));