using System.IO;
using System.Collections.Generic;

var reader = new StreamReader("d22.txt");
Queue<int> p1 = new();
Queue<int> p2 = new();

Queue<int> refQueue = p1;
reader.ReadLine();

do {
    string line = reader.ReadLine();
    if(line is {Length: > 0}){
        if(line[0]=='P'){
            refQueue = p2;
        }
        else{
            refQueue.Enqueue(int.Parse(line));
        }
    }
} while(!reader.EndOfStream);

while(p1.Any() && p2.Any()){
    int n1 = p1.Dequeue();
    int n2 = p2.Dequeue();
    if(n1 > n2)
    {
        p1.Enqueue(n1);
        p1.Enqueue(n2);
    }
    else if(n1 < n2)
    {
        p2.Enqueue(n2);
        p2.Enqueue(n1);
    }
}

if(p1.Any()){
    int calcIndex = p1.Count;
    Console.WriteLine(p1.Aggregate(0, (acc,item)=>acc+item*(calcIndex--)));
}
else{
    int calcIndex = p2.Count;
    Console.WriteLine(p2.Aggregate(0, (acc,item)=>acc+item*(calcIndex--)));
}