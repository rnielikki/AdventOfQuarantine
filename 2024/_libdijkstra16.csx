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
        while (_priorities.Count > 0)
        {
            var current = _priorities.Dequeue();
            //fast visits first
            if (_visited.Contains(current.Coordinate)) continue;
            _visited.Add(current.Coordinate);
            if (current.Coordinate.Eq(_end))
            {
                return current;
            }
            else FindPathAround(current);
        }
        return PathInfo.Empty;
    }
    public HashSet<Coordinate> FindAll()
    {
        Dictionary<Coordinate, VisitedData> visited2 = new();
        _priorities.Enqueue(new PathInfo(_start), 0);
        while (_priorities.Count > 0)
        {
            var current = _priorities.Dequeue();
            if (visited2.TryGetValue(current.Coordinate, out var res))
            {
                var contains = visited2.ContainsKey(current.Coordinate + res.Direction);
                if(//(res.Direction.Eq(-current.Direction) && res.Priority == current.GetPriority())||
                (res.Priority+1000 == current.GetPriority()))
                {
                    if(!contains) res.Parents.Add(current.Paths[^1]);
                    //res.Parents.Add(current.Paths[^1]);
                }
                continue;
            }
            var visitedData = (current.Paths.Count > 0)?
                (new VisitedData(current.GetPriority(), current.Paths[^1], current.Direction))
                :(new VisitedData(current.GetPriority(), current.Direction));
            visited2.Add(current.Coordinate, visitedData);
            if (current.Coordinate.Eq(_end))
            {
                return GetResult(visited2, _end);
            }
            else FindPathAround(current);
        }
        throw new Exception("lol");
        //return results;
    }
    private HashSet<Coordinate> GetResult(Dictionary<Coordinate, VisitedData> visited2, Coordinate current)
    {
        var result = new HashSet<Coordinate>();
        result.Add(current);
        if(visited2.TryGetValue(current, out var res) && res.Parents.Count == 0)
        {
            return result;
        }
        foreach(var parent in res.Parents)
        {
            foreach(var r in GetResult(visited2, parent))
            {
                result.Add(r);
            }
        }
        return result;
    }

    private void FindPathAround(PathInfo pathInfo)
    {
        foreach (var dir in Directions.All)
        {
            var newCoord = pathInfo.Coordinate + dir;
            if (!pathInfo.Paths.Contains(newCoord) && IsValid(newCoord, _data))
            {
                var priority = pathInfo.GetPriority(newCoord, _end, dir);
                _priorities.Enqueue(new PathInfo(newCoord, pathInfo), priority);
            }
        }
    }
    protected virtual bool IsValid(Coordinate coord, bool[,] data) => coord.CanPass(_data);
}

struct PathInfo
{
    public Coordinate Coordinate { get; set; }
    public readonly List<Coordinate> Paths;
    //debug
    public static PathInfo Empty => new PathInfo(new Coordinate(0, 0));
    public int TurnTimes { get; private set; }
    public Coordinate Direction { get; private set; }
    public PathInfo(Coordinate coord)
    {
        Coordinate = coord;
        Paths = new();
        Direction = Directions.Zero;
        TurnTimes = 0;
    }
    public PathInfo(Coordinate coord, PathInfo oldPathInfo)
    {
        Coordinate = coord;
        Direction = coord-oldPathInfo.Coordinate;
        TurnTimes = oldPathInfo.TurnTimes;
        Paths = oldPathInfo.Paths.ToList();
        Paths.Add(oldPathInfo.Coordinate);
        if (!Direction.Eq(oldPathInfo.Direction))
        {
            TurnTimes++;
        }
    }
    //leveys ensin haku: 🍞th  first search
    //var priority = pathInfo.Paths.Count+1;
    //by subtracting distance, ensures the path is right way, A* (but lacks of priority update :/)
    public int GetPriority(Coordinate next, Coordinate end, Coordinate direction)
    {
        bool turned = false;
        if (!direction.Eq(Direction))
        {
            turned = true;
        }
        return GetPriority(Paths.Count, (TurnTimes + (turned?1:0)));
    }
    public int GetPriority()
        => GetPriority(Paths.Count, TurnTimes);
    public int GetPriority(int pathCounts, int turnTimes)
        => pathCounts + turnTimes * 1000;
}
struct VisitedData
{
    public int Priority { get; }
    public readonly List<Coordinate> Parents = new();
    public Coordinate Direction { get; }
    public VisitedData(int priority, Coordinate parent, Coordinate direction)
    {
        Priority = priority;
        Parents.Add(parent);
        Direction = direction;
    }
    public VisitedData(int priority, Coordinate direction)
    {
        Priority = priority;
        Direction = direction;
    }
}