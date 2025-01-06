#load  "./_libcoord.csx"

class PathFinder
{
    readonly PriorityQueue<PathInfo, int> _priorities;
    readonly HashSet<Coordinate> _visited = new();
    
    Coordinate _start;
    Coordinate _end;
    private bool[,] _data;
    //true: is wall, false: can pass
    public PathFinder(bool[,] data, Coordinate start, Coordinate end)
    {
        _priorities = new();
        _data = data;
        _start = start;
        _end = end;
    }
    public PathFinder(bool[,] data, Coordinate start, Coordinate end, IComparer<int> comparer)
    {
        _priorities = new(comparer);
        _data = data;
        _start = start;
        _end = end;
    }
    public PathInfo Find()
    {
        _priorities.Enqueue(new PathInfo(_start), 0);
        while(_priorities.Count > 0)
        {
            var current = _priorities.Dequeue();
            //fast visits first
            if(_visited.Contains(current.Coordinate)) continue;
            _visited.Add(current.Coordinate);
            if(current.Coordinate.Eq(_end)) return current;
            FindPathAround(current);
        }
        return PathInfo.Empty;
    }

    private void FindPathAround(PathInfo pathInfo)
    {
        foreach(var dir in Directions.All)
        {
            var newCoord = pathInfo.Coordinate + dir;
            if(!_visited.Contains(newCoord) && IsValid(newCoord, _data))
            {
                //leveys ensin haku: 🍞th  first search
                //var priority = pathInfo.Paths.Count+1;
                //by subtracting distance, ensures the path is right way, A* (but lacks of priority update :/)
                var priority = pathInfo.GetPriority(newCoord, _end);
                _priorities.Enqueue(new PathInfo(newCoord, pathInfo), priority);
                //dijkstra:
                //1.checks if `newCoord` in `_priorityQueue`
                //2. replace... What? C# PriorityQueue doesn't support priority update?!
            }
        }
    }
    protected virtual bool IsValid(Coordinate coord, bool[,] data) => coord.CanPass(_data);
}

struct PathInfo
{
    public Coordinate Coordinate { get; set; }
    public readonly List<Coordinate> Paths;
    public static PathInfo Empty => new PathInfo(new Coordinate(0,0));
    public PathInfo(Coordinate coord)
    {
        Coordinate = coord;
        Paths = new();
    }
    public PathInfo(Coordinate coord, PathInfo oldPathInfo)
    {
        Coordinate = coord;
        Paths = oldPathInfo.Paths.ToList();
        Paths.Add(coord);
    }
    //leveys ensin haku: 🍞th  first search
    //var priority = pathInfo.Paths.Count+1;
    //by subtracting distance, ensures the path is right way, A* (but lacks of priority update :/)
    public int GetPriority(Coordinate next, Coordinate end) =>
        end.X - next.X + end.Y - next.Y + Paths.Count + 1;
}