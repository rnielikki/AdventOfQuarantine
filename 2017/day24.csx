using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

Node.Init();
Console.WriteLine(Node.FirstPart());
Console.WriteLine(Node.SecondPart());

class Node{
    private static List<Node> _allNodes;
    private static List<Path> _paths = new();
    public int StartPort { get; init;}
    public int EndPort { get; init; }
    public Node(int start, int end){
        StartPort = start;
        EndPort = end;
    }

    public static void Init(){
        _allNodes = new List<Node>();
        foreach(var l in File.ReadLines("day24.txt")){
            var n = l.Split('/');
            _allNodes.Add(new Node(int.Parse(n[0]), int.Parse(n[1])) );
        }
        foreach(var nd in _allNodes.Where(item=>item.StartPort == 0)){
            nd.FindAndSet(new Path(nd), nd.EndPort);
        }
    }

    private void FindAndSet(Path path, int openPart){
        //Console.WriteLine($"{StartPort}, {EndPort} / {openPart}");
        if(StartPort != openPart && EndPort != openPart) throw new ArgumentException($"port {openPart} not matches {StartPort}/{EndPort}");

        var range = _allNodes.Select(item=>CheckPort(path, openPart, item)).Where(item=>item.Item1!=null);
        if(!range.Any()){ _paths.Add(path); }
        foreach(var r in range){
            var (subnode, open) = r;
            var p = new Path(path);
            p.Add(subnode);
            subnode.FindAndSet(p, open);
        }
    }

    private (Node, int) CheckPort(Path path, int side, Node another){
        if(another == this || path.Contains(another)) return (null, -1);
        else if(another.StartPort == side) return (another, another.EndPort);
        else if(another.EndPort == side) return (another, another.StartPort);
        else return (null, -1);
    }
    public static int FirstPart()
    {
        return _paths.Select(item=>item.Sum).Max();
    }
    public static int SecondPart()
    {
        return _paths.Aggregate((acc, item)=>acc.Size>item.Size?acc:item).Sum;
    }
}
class Path{
    HashSet<Node> Nodes = new();
    public int Sum { get => Nodes.Sum(n=>n.StartPort+n.EndPort); }
    public int Size { get => Nodes.Count; }
    public Path(Node node){
        Nodes.Add(node);
    }
    public Path(Path path):this(path.Nodes){}
    public Path(IEnumerable<Node> nodes){
        Nodes =  nodes.ToHashSet();
    }
    public void Add (Node node) => Nodes.Add(node);
    public bool Contains(Node node) => Nodes.Contains(node);
}