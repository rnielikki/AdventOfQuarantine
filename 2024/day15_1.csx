bool secondReading = false;
Coordinate current = new Coordinate {X = 0, Y = 0};
Coordinate robot = new Coordinate {X = 0, Y = 0};
List<Coordinate> instructions = new();
Coordinate[] allDirections = [
new Coordinate(){X=0, Y=-1},
new Coordinate(){X=1, Y=0},
new Coordinate(){X=0, Y=1},
new Coordinate(){X=-1, Y=0}
];
Dictionary<char, Coordinate> dirByChar = new(){
    {'^', allDirections[(int)Directions.up]},
    {'>', allDirections[(int)Directions.right]},
    {'v', allDirections[(int)Directions.down]},
    {'<', allDirections[(int)Directions.left]},
};
enum Directions {
    up, right, down, left
}
char[][] Generate(){
    List<char[]> coordsList = new();
    int currentY = 0;

    foreach(var line in File.ReadAllLines("day15.txt")) {
        if(string.IsNullOrEmpty(line)) {
            secondReading = true;
            continue;
        }
        if(!secondReading) {
            int robotIndex = line.IndexOf('@');
            if(robotIndex>-1) {
                robot.X = robotIndex;
                robot.Y = currentY;
            }
            coordsList.Add(line.ToCharArray());
            currentY++;
        }
        else {
            foreach(var c in line) {
                if(dirByChar.TryGetValue(c, out var dir)) {
                    instructions.Add(dir);
                }
            }
        }
    }
    return coordsList.ToArray();
}
var data = Generate();
//PrintData(data);
foreach(var inst in instructions) {
    MoveRobot(data, inst);
}
long sum=0;
for(int y=0;y<data.Length;y++) {
    for(int x=0;x<data.Length;x++) {
        if(data[y][x] == 'O') {
            sum+=100*y+x;
        }
    }
}
Console.WriteLine(sum);
void MoveRobot(char[][] input, Coordinate direction) {
    var nextPos = robot+direction;
    switch(input[nextPos.Y][nextPos.X]) {
       case '.' :
       case 'O':
           if(MoveItem(input, robot, nextPos, direction)) {
                robot = nextPos;
           }
           break;
        case '#':
           break;
        default:
           throw new InvalidOperationException();
    }
}

bool MoveItem(char[][] input, Coordinate from, Coordinate to, Coordinate direction) {
    if(input[to.Y][to.X] != '.') {
        if(input[to.Y][to.X]=='O') {
            if(MoveItem(input, to, to+direction, direction)) {
                Move();
                return true;
            }
            else return false;
        }
        return false;
    }
    Move();
    return true;
    void Move() {
        input[to.Y][to.X] = input[from.Y][from.X];
        input[from.Y][from.X] = '.';
    }
}



void PrintData(char[][] input) {
    for(int y=0;y<input.Length;y++) {
        for(int x=0;x<input[0].Length;x++) {
            Console.Write(input[y][x]);
        }
        Console.WriteLine();
    }
}

struct Segment {
    public bool IsWall { get ; set; }
    public Coordinate Coord { get ; set; }
}

//umm.csx
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