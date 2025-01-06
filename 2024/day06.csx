(int y, int x) guardPos = new (-1, -1);
(int y, int x) firstGuardPos = new (-1, -1);
const char obstacle = '#';
const char neutral = '.';
const char guardInit = '^';
HashSet<(int y, int x)> path = new ();

(int y, int x)[] allDirections = new (int, int)[4]{
    (-1, 0),
    (0, 1),
    (1, 0),
    (0, -1)
};
//why are you putting enum here for just ummmmm some small script loll
enum Direction
{
    Up,
    Right,
    Down,
    Left
}

int lineNumner = 0;
char[][] data = File.ReadAllLines("day06.txt").Select(c =>
{
    int guardIndex = c.IndexOf(guardInit);
    if (guardIndex > -1)
    {
        if(firstGuardPos.y > -1) throw new FormatException("Guard can be only one!");
        firstGuardPos.y = lineNumner;
        firstGuardPos.x = guardIndex;
    }
    lineNumner++;
    return c.ToCharArray();
}).ToArray();

guardPos = firstGuardPos;
path.Add(guardPos);

Direction currentDirection = Direction.Up;
(int y, int x) direction = (-1, 0);

while(true)
{
    (int y, int x) nextPos = (guardPos.y + direction.y, guardPos.x + direction.x);
    if(IsOutOfBound(nextPos.y, nextPos.x)) break;
    if(data[nextPos.y][nextPos.x]==obstacle)
    {
        int nextDir = ((int)(currentDirection)+1)%allDirections.Length;
        currentDirection = (Direction)nextDir;
        direction = allDirections[nextDir];
        nextPos = (guardPos.y + direction.y, guardPos.x + direction.x);
        if(IsOutOfBound(nextPos.y, nextPos.x)) break;
    }
    if(!path.Contains(nextPos)) path.Add(nextPos);
    guardPos = nextPos;
}

bool IsOutOfBound(int y, int x) => IsOutOfBoundX(x) || IsOutOfBoundY(y);

bool IsOutOfBoundX(int x) => x >= data[0].Length || x < 0;
bool IsOutOfBoundY(int y) => y >= data.Length || y < 0;


HashSet<(int y, int x, Direction direction)> turningInfo = new ();
int obstacleCount = 0;

//--- necessary to save the whole path first because... setting obstacle on non-path means nothing

//1. save direction info to dictionary ONLY when it turns, MUST save current guard position
//2. if it goes out of bound, just break
//3. if found add count and break

foreach(var test in path)
{
    //set obstacle
    data[test.y][test.x] = obstacle;
    //init
    turningInfo.Clear();
    guardPos = firstGuardPos;
    currentDirection = Direction.Up;
    direction = (-1, 0);

    //copypasta code starts here--------------------
    while(true)
    {
        (int y, int x) nextPos = (guardPos.y + direction.y, guardPos.x + direction.x);
        if(IsOutOfBound(nextPos.y, nextPos.x)) break;

        //yep 2+ turns in 1 place possible
        bool turned = false;
        while(data[nextPos.y][nextPos.x]==obstacle)
        {
            turned = true;

            int nextDir = ((int)(currentDirection)+1)%allDirections.Length;
            currentDirection = (Direction)nextDir;
            direction = allDirections[nextDir];
           
            nextPos = (guardPos.y + direction.y, guardPos.x + direction.x);
            if(IsOutOfBound(nextPos.y, nextPos.x)) break;
        }
        if(turned)
        {
            if(turningInfo.Contains((guardPos.y, guardPos.x, currentDirection)))
            {
                obstacleCount++;
                break;
            }
            turningInfo.Add((guardPos.y, guardPos.x, currentDirection));

        }
        guardPos = nextPos;
    }

    //put down back after checking
    data[test.y][test.x] = neutral;
}
Console.WriteLine(obstacleCount);
turningInfo.Clear();