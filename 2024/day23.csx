var data = File.ReadAllLines("day23.txt").Where(a=>!string.IsNullOrWhiteSpace(a)).Select(a => a.Split("-"));

Dictionary<string, Node> everyNodes = new();

foreach(var d in data)
{
    AddKey(d[0], d[1]);
}

//---------------------------- [PART 1] ---------------------------//
int sum = 0;
HashSet<string> tried = new();
foreach(var (key, node) in everyNodes.Where(kv => kv.Key.StartsWith('t')))
{
    //Console.WriteLine($"{key}, {string.Join(",",node.GetAllKeys())}");
    sum+=CountTriangles(node);
}
Console.WriteLine(sum);

int CountTriangles(Node node)
{
    int count = 0;
    foreach(Node second in node.GetNeighbours())
    {
        foreach(var third in second.GetNeighbours().Where(v => v.Has(node.Key)))
        {
            var keys = new[]{node.Key, second.Key, third.Key};
            string triangleKey = string.Join("",keys.OrderBy(s=>s));
            if(!tried.Contains(triangleKey))
            {
                count++;
                tried.Add(triangleKey);
            }
        }
    }
    return count;
}
//---------------------------- [PART 2] ---------------------------//

//Dictionary<string, int> everyNodeCount = new();
//if bigger than max node, break
var maxLen = everyNodes.Values.Max(s => s.GetNeighboursCount());
List<List<Node>> resultList = new();
foreach(var (_, node) in everyNodes)
{
    var res = CountLevel(node);
    if(res.Count!=0) resultList.Add(res);
}
Console.WriteLine(GetKey(resultList.MaxBy(a => a.Count)));

List<Node> CountLevel(Node node)
{
    List<Node> result = new();
    result.Add(node);
    var neighbours = node.GetNeighbours();
    foreach(var neighbour in neighbours)
    {
        //don't ask why this magic works
        if(neighbour.GetNeighbours().Except(neighbours).Count() == 2)
        {
            result.Add(neighbour);
        }
    }
    return result;
}

string GetKey(IEnumerable<Node> nodes) => string.Join(',',nodes.Select(n => n.Key).OrderBy(s => s));


//---------------------------------------------------------------------- COMMON CODES
void AddKey(string key, string next)
{
    if(!everyNodes.TryGetValue(key, out var node1))
    {
        node1 = new Node(key);
        everyNodes.Add(key, node1);
    }
    if(!everyNodes.TryGetValue(next, out var node2))
    {
        node2 = new Node(next);
        everyNodes.Add(next, node2);
    }
    node1.Add(node2);
    node2.Add(node1);
}

//----------------------- class
class Node {
    public string Key { get; private set; }
    private readonly Dictionary<string,Node> Neighbours = new();
    public Node(string key) => Key = key;
    
    public bool Has(string key) => Neighbours.ContainsKey(key);
    public bool TryGet(string key, out Node node) => Neighbours.TryGetValue(key, out node);
    public void Add(Node node)
    {
        if(node.Key == Key) return;
        if(!Neighbours.ContainsKey(node.Key)) Neighbours.Add(node.Key, node);
    }
    //debug purpose
    public IEnumerable<string> GetAllKeys() => GetNeighbours().Select(v => v.Key);
    public IEnumerable<Node> GetNeighbours() => Neighbours.Values;
    public int GetNeighboursCount() => Neighbours.Count;
}