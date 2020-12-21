using System;
using System.IO;

struct Point {
    public int X { get; set;}
    public int Y { get; set; }
    public Point(int x, int y){
        X = x; Y = y;
    }
    public void Rotate(int right)
    {
        int angleScale = (4+right)%4;
        switch(angleScale){
            case 0:
                break;
            case 1:
                int temp = Y;
                Y = -X;
                X = temp;
                break;
            case 2:
                X = -X;
                Y = -Y;
                break;
            case 3:
                int __temp = Y;
                Y = X;
                X = -__temp;
                break;
        }
    }
}

class ShipStatus {
    char[] directions = new char[]{'N', 'E', 'S', 'W'};
    private Point _point;
    private Point _wayPoint;
    public ShipStatus(int x, int y)
    {
        _point = new Point(x,y);
        _wayPoint = new Point(10,1);
    }
    public ShipStatus SetDirection(int scale)
    {
        _wayPoint.Rotate(scale);
        return this;
    }
    public ShipStatus MoveX(int x)
    {
        _wayPoint.X += x;
        return this;
    }
    public ShipStatus MoveY(int y)
    {
        _wayPoint.Y += y;
        return this;
    }
    public ShipStatus Move(int scale){
        _point.X += _wayPoint.X * scale;
        _point.Y += _wayPoint.Y * scale;
        return this;
    }
    public override string ToString()
    {
        return $"Current {_point.X}, {_point.Y} / Waypoint {_wayPoint.X}, {_wayPoint.Y}";
    }
    public int GetResult() =>
        Math.Abs(_point.X)+Math.Abs(_point.Y);
  }

ShipStatus ChangeStatus(char direction, int scale, ShipStatus status) => direction switch {
    'E' => status.MoveX(1*scale),
    'W' => status.MoveX(-1*scale),
    'N' => status.MoveY(1*scale),
    'S' => status.MoveY(-1*scale),
    'F' => status.Move(scale),
    'L' => status.SetDirection(-scale/90),
    'R' => status.SetDirection(scale/90),
    _ => throw new ArgumentException()
};

var str = new StreamReader("d12.txt");
ShipStatus status = new ShipStatus(0, 0);
while(!str.EndOfStream){
    string line = str.ReadLine();
    (char c, int i) = Parse(line);
    ChangeStatus(c, i, status);
    Console.Write(line+" :: ");
    Console.WriteLine(status.ToString());
}
Console.WriteLine(status.GetResult());
(char, int) Parse(string str) => (str[0], int.Parse(str.Substring(1)));