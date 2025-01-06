#load  "./_libdijkstra.csx"

HashSet<Coordinate> didIVisit = new();
var raw = File.ReadAllLines("day20.txt");
var height = raw[0].Length;
var width = raw.Length;
const char wall = '#';
bool[,] data = new bool[width, height];

const int cheatGoal = 100;
const int fullCheatTime = 20;
long res = 0;

Coordinate startp, endp;

for(int y=0;y<height;y++)
{
    for(int x=0;x<width;x++)
    {
        switch(raw[y][x])
        {
            case wall:
                data[x,y] = true;
                break;
            case 'S':
                startp = new Coordinate(x,y);
                break;
            case 'E':
                endp = new Coordinate(x,y);
                break;
            default:
                break;
        }
    }
}

var pf = new PathFinder(data, startp, endp);
var r = pf.Find();

var pathLenghts = new Dictionary<Coordinate,int>();

pathLenghts.Add(startp,0);

for(int i=0; i<r.Paths.Count; i++) {
    pathLenghts.Add(r.Paths[i],i+1);
}

for(int y=1;y<height-1;y++)
{
    for(int x=1;x<width-1;x++)
    {
        if(!data[x,y])
        {
            for(int cheatTime=2;cheatTime<=fullCheatTime;cheatTime++){
            //var cheatTime = 2;
                for(int i=0;i<=cheatTime;i++)
                {
                    CheckEach(x, y, i, cheatTime-i, cheatTime);
                    if(i!=0) CheckEach(x, y, -i, cheatTime-i, cheatTime);
                    if(i!=cheatTime)
                    {
                        CheckEach(x, y, -i, i-cheatTime, cheatTime);
                        if(i!=0) CheckEach(x, y, i, i-cheatTime, cheatTime);
                    }
                }
            }
        }
    }
}
void CheckEach(int x, int y, int xOffset, int yOffset, int cheatUsed)
{
    var start = new Coordinate(x, y);
    var end = new Coordinate(x + xOffset, y + yOffset);
    if(didIVisit.Contains(end)) return;
    didIVisit.Add(start);
    //invalid or wall
    if (!end.CanPass(data)) return;
    var startLength = pathLenghts[start];
    var endLength = pathLenghts[end];

    var count = Math.Abs(startLength - endLength) - cheatUsed;

    if (count >= cheatGoal) {
        res++;
    }
}
Console.WriteLine(res);