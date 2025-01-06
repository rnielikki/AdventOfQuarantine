public struct Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
    public static Coordinate operator +(Coordinate coord1, Coordinate coord2) => new Coordinate() { X = coord1.X + coord2.X, Y = coord1.Y + coord2.Y };
    public static Coordinate operator -(Coordinate coord1, Coordinate coord2) => new Coordinate() { X = coord1.X - coord2.X, Y = coord1.Y - coord2.Y };
    public static Coordinate operator *(Coordinate coord1, int multiplier) => new Coordinate() { X = coord1.X *multiplier, Y = coord1.Y *multiplier };
    public static Coordinate operator -(Coordinate coord) => new Coordinate() { X = -coord.X, Y = -coord.Y };
    public bool Eq(Coordinate coord2) => X == coord2.X && Y == coord2.Y;
    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }
    public override string ToString() => $"({X}, {Y})";

    public bool IsInvalid(bool[,] data) =>
        X < 0 || Y < 0 || X >= data.GetLength(0) || Y >= data.GetLength(1);
    public bool CanPass(bool[,] data) => !IsInvalid(data) && !data[X,Y];
}
public static class Directions
{
    public static Coordinate[] All = new Coordinate[4]
    {
    new Coordinate(-1, 0),
    new Coordinate(1, 0),
    new Coordinate(0, -1),
    new Coordinate(0, 1)
    };
    public static Coordinate Zero = new Coordinate(0,0);
    public static Dictionary<DirectionType, Coordinate> ByName =
    new(){
        {DirectionType.Left, new Coordinate(-1, 0)},
        {DirectionType.Right, new Coordinate(1, 0)},
        {DirectionType.Up, new Coordinate(0, -1)},
        {DirectionType.Down, new Coordinate(0, 1)},
    };
}
public enum DirectionType
{
    None,
    Up,
    Down,
    Left,
    Right
}