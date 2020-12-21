using System.IO;
using System.Collections.Generic;
using System.Linq;

var __lines = File.ReadAllLines("d13.txt");
var __d1= __lines[1].Split(',');
var busAndTime = new List<(int,int)>();
int firstItem = int.Parse(__d1[0]);
int __busCount = 1;
foreach(var d in __d1)
{
    if(d!=__d1[0]){
        if(int.TryParse(d, out int res)){
            busAndTime.Add((res, __busCount++));
        }
        else{
            __busCount++;
        }
    }
}
foreach(var x in busAndTime){
    Console.WriteLine(x);
}

long cycle = firstItem;
long t = firstItem;
foreach(var item in busAndTime)
{
    Console.WriteLine(item.Item1);
    while(!Check(t,item.Item1, item.Item2)) {
        t += cycle;
    }
    cycle *= item.Item1;
}

Console.WriteLine(t);

/*for(long pos=0;;pos+=cycle)
{
    
    if(busAndTime.All(item => Check(pos, item.Item1, item.Item2))){
        Console.WriteLine(pos);
        break;
    }
}*/

bool Check(long position, int bus, int distance) => (bus-(distance%bus) == position%bus)?true:false;