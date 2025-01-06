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
                robot.X = robotIndex*2;
                robot.Y = currentY;
            }
            coordsList.Add(line.Select(c=> c switch{
                   '#' => "##".ToCharArray(),
                   '.' => "..".ToCharArray(),
                   '@' => "@.".ToCharArray(),
                   'O' => "[]".ToCharArray(),
                    _ => throw new Exception()
            }
            ).SelectMany(c=>c).ToArray());
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
foreach(var inst in instructions) {
    MoveRobot(data, inst);
}
long sum=0;
for(int y=0;y<data.Length;y++) {
    for(int x=0;x<data[0].Length;x++) {
        if(data[y][x] == '[') {
            sum+=100*y+x;
        }
    }
}
Console.WriteLine(sum);

void MoveRobot(char[][] input, Coordinate direction) {
    var nextPos = robot+direction;
    switch(input[nextPos.Y][nextPos.X]) {
       case '.' :
       case '[':
       case ']':
           if(MoveItem(input, robot, nextPos, direction)) {
                robot = nextPos;
           }
           break;
        case '#':
           break;
        default:
           throw new InvalidOperationException(input[nextPos.Y][nextPos.X].ToString());
    }
}

bool MoveItem(char[][] input, Coordinate from, Coordinate to, Coordinate direction) {
    var destValue = input[to.Y][to.X];
    if(destValue != '.') {
        if(destValue=='[' || destValue == ']') {
            if(direction.Y==0) {
                 if(MoveItem(input, to, to+direction, direction)) {
                    Move(input, from, to);
                    return true;
                }
                 else return false;
            }
            else {
                bool canMove = MoveTest(input, to, direction);
                if(canMove) {
                    MoveChain(input, from, to, direction);
                    return true;
                }
                else return false;
            }
        }
        return false;
    }
    Move(input, from, to);
    return true;
}

bool MoveTest(char[][] input, Coordinate to, Coordinate direction) {
    var destValue = input[to.Y][to.X];
    if(destValue != '.') {
        if(destValue=='[' || destValue == ']') {
            bool valid = true;
            var neighbour = to;
            neighbour.X += (destValue=='[')?1:-1;
            valid &= MoveTest(input, to+direction, direction);
            valid &= MoveTest(input, neighbour+direction, direction);
            return valid;
        }
        return false;
    }
    else return true;
}
void MoveChain(char[][] input, Coordinate from, Coordinate to, Coordinate direction) {
    var destValue = input[to.Y][to.X];
    if(destValue != '.') {
        if(destValue=='[' || destValue == ']') {
            var neighbour = to;
            neighbour.X += (destValue=='[')?1:-1;
            MoveChain(input, to, to+direction, direction);
            MoveChain(input, neighbour, neighbour+direction, direction);
            Move(input, from, to);
        }
    }
    else Move(input, from, to);
}


void Move(char[][] input, Coordinate from, Coordinate to) {
    input[to.Y][to.X] = input[from.Y][from.X];
    input[from.Y][from.X] = '.';
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