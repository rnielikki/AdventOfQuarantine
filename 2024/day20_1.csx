#load  "./_libdijkstra.csx"

var raw = File.ReadAllLines("day20.txt");
var height = raw[0].Length;
var width = raw.Length;
const char wall = '#';
bool[,] data = new bool[width, height];

const int cheatGoal = 100;

Coordinate start, end;

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
                start = new Coordinate(x,y);
                break;
            case 'E':
                end = new Coordinate(x,y);
                break;
            default:
                break;
        }
    }
}
int cheating = 0;
//prevents out of index
for(int x=1;x<width-1;x++)
{
    for(int y=1;y<height-1;y++)
    {
        if(data[x,y]==false) continue;
//horizontal check
        if((!data[x+1,y] && !data[x-1, y]) // around x (can pass through x)
            && (data[x, y+1] && data[x, y-1])) //shouldn't have opened y sides, grants a bit less task
            {
               var res =new PathFinder(data, new Coordinate(x-1, y), new Coordinate(x+1, y)).Find();
               if(res.Paths.Count>0)
               {
                    var cheatAmount = res.Paths.Count-2; //-2 is necessary because you get two steps with cheating
                   //do something
                   if(cheatAmount>=cheatGoal)
                   {
                       //Console.WriteLine(cheatAmount + " from horizontal "+cheating);
                       cheating++;
                   }
               }
            }
//vertical check
        if((!data[x,y+1] && !data[x, y-1]) // around y (can pass through y)
            && (data[x+1,y] && data[x-1, y])) //shouldn't have opened x sides, grants a bit less task
            {
               var res =new PathFinder(data, new Coordinate(x, y-1), new Coordinate(x, y+1)).Find();
               if(res.Paths.Count>0)
               {
                    var cheatAmount = res.Paths.Count-2; //-2 is necessary because you get two steps with cheating
                   //do something
                   if(cheatAmount>=cheatGoal)
                   {
                       //Console.WriteLine(cheatAmount + " from vertical "+cheating);
                       cheating++;
                   }
               }
            }
    }
}
Console.WriteLine(cheating);