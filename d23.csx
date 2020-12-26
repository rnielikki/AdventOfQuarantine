using System;
using System.Linq;
var input = "963275481".ToCharArray().Select(i=>int.Parse(i.ToString()));

IEnumerable<int> NextOfOne(IEnumerable<int> values, int repeat, int amount){
    HashSet<Node> nodes = new();
    Node[] pickups = new Node[3];
    
    Node currentCup;
    {
        Node before = new Node{ Value = values.First() };
        currentCup = before;
        foreach(var v in values.Skip(1)){
            var node = new Node{
                Value = v
            };
            before.After = node;
            nodes.Add(before);
            before = node;
        }
        //circular
        before.After = currentCup;
        nodes.Add(before);
    }
    int CupLength = nodes.Count;
    for(int r=0;r<repeat;r++){
        //1. clockwise
        Node n = currentCup;
        for(int i=0;i<3;i++){
            pickups[i] = n.After;
            n = pickups[i];
        }

        //2. destination
        int dest = currentCup.Value;
        do {
            dest = (CupLength+dest-2)%CupLength+1;
        } while(pickups.Any(v=>v.Value == dest));

        //3. Places the cups
        var afterCurrent = pickups[2].After;
        currentCup.After = afterCurrent;

        var destNode = FindNode(dest);

        pickups[2].After = destNode.After;
        destNode.After = pickups[0];

        //4. New current cup
        currentCup = currentCup.After;
    }
    
    var result = new List<int>();
    var resultNode = FindNode(1);
    for(int i=0;i<amount;i++){
        result.Add(resultNode.After.Value);
        resultNode = resultNode.After;
    }
    return result;
    Node FindNode(int number){
        if(!nodes.TryGetValue(new Node{ Value = number}, out Node node)) throw new KeyNotFoundException("not found "+number);
        return node;
    }
}
class Node{
    public Node After {get; set;}
    public int Value {get;set;}
    public override int GetHashCode()
    {
        return Value;
    }
    public override bool Equals(object obj)
    {
        return ((Node)obj).Value == Value;
    }
}


//answer1
foreach(var asdf in NextOfOne(input, 100, 8))
    Console.Write(asdf);

Console.WriteLine();

//-------------------------------------------

var v2 = NextOfOne(input.Concat(Enumerable.Range(10, 1000000-9)), 10000000, 2);
Console.WriteLine((long)(v2.First())*(long)(v2.Last()));
