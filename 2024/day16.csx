#load  "./_libdijkstra16.csx"

var raw = File.ReadAllLines("day16.txt");
var height = raw[0].Length;
var width = raw.Length;
const char wall = '#';
bool[,] data = new bool[width, height];

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
var result = (new PathFinder(data, start, end)).Find();
//Visualise(result.Paths, result.TurnPaths);
Console.WriteLine(result.GetPriority());
Console.WriteLine(result.Paths.Count);
var result2_1 = (new PathFinder(data, start, end)).FindAll();
var result2_2 = (new PathFinder(data, end,start)).FindAll();
var result2 = result2_1.Union(result2_2).ToHashSet();
Console.WriteLine(result2.Count);

//Visualise(result2, Enumerable.Empty<Coordinate>());

void Visualise(IEnumerable<Coordinate> path, IEnumerable<Coordinate> turnPath)
{
    for(int y=0;y<height;y++)
    {
        for(int x=0;x<width;x++)
        {
            var c = new Coordinate(x, y);
            if(path.Contains(c))
            {
                Console.Write(turnPath.Contains(c)?'X':'O');
            }
            else Console.Write(raw[y][x]);
        }
        Console.WriteLine();
    }
}