using System.IO;
using System.Collections.Generic;

enum Player { P1, P2 };
var reader = new StreamReader("d22.txt");
Queue<int> realP1 = new();
Queue<int> realP2 = new();

Queue<int> refQueue = realP1; //reference for queueing (while reading from file)
reader.ReadLine();

do
{
    string line = reader.ReadLine();
    if (line is { Length: > 0 })
    {
        if (line[0] == 'P')
        {
            refQueue = realP2;
        }
        else
        {
            refQueue.Enqueue(int.Parse(line));
        }
    }
} while (!reader.EndOfStream);

//inside loop-----------------------------------
///<summary>Starts the Combat.</summary>
///<returns>Winner player of the game</returns>
Player RecursiveCombat(Queue<int> currentP1, Queue<int> currentP2, bool copy=true){
    List<IEnumerable<int>> previousP1 = new();
    List<IEnumerable<int>> previousP2 = new();
    Queue<int> p1, p2;
    if(copy){
        p1 = new Queue<int>(currentP1);
        p2 = new Queue<int>(currentP2);
    }
    else{
        p1 = currentP1;
        p2 = currentP2;
    }
    while (p1.Any() && p2.Any())
    {
        int n1 = p1.First();
        int n2 = p2.First();
        if (
        (previousP1.Any(item => item.SequenceEqual(p1))
        || previousP2.Any(item => item.SequenceEqual(p2)))
        )
        {
            //gameEnds for p1.
            return Player.P1;
        }
        else
        {
            previousP1.Add(p1.ToArray());
            previousP2.Add(p2.ToArray());
        }
        p1.Dequeue();
        p2.Dequeue();
        if (n1 <= p1.Count && n2 <= p2.Count)
        {
            switch(RecursiveCombat(new(p1.Take(n1)), new(p2.Take(n2)) )){
                case Player.P1:
                    SetCard(p1, n1, n2);
                    break;
                case Player.P2:
                    SetCard(p2, n2, n1);
                    break;
            }
        }
        else
        {
            //if recursive combat not valid
            if(n1 > n2)
            {
                SetCard(p1, n1, n2);
            }
            else if(n1 < n2)
            {
                SetCard(p2, n2, n1);
            }
        }
    }
    if(p1.Any()) return Player.P1;
    else return Player.P2;
}
//loop ends----------------------------
void SetCard(Queue<int> winner, int first, int second){
    winner.Enqueue(first);
    winner.Enqueue(second);
}

Console.WriteLine(RecursiveCombat(realP1, realP2, false));
var winningQueue = realP1.Any()?realP1:realP2;
int calcIndex = winningQueue.Count;
Console.WriteLine(winningQueue.Aggregate(0, (acc,item)=>acc+item*(calcIndex--)));
Console.WriteLine("--------------**** res *** ---------------------");

foreach(var a in realP1){
    Console.WriteLine(a);
}
Console.WriteLine("--------------");
foreach(var a in realP2){
    Console.WriteLine(a);
}