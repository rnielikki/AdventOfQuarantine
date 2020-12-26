using System.IO;

var raw = File.ReadAllLines("d25.txt").Select(val=>int.Parse(val));
int card = raw.First();
int door = raw.Last();
long val = 1;
int loop = 0;
while(val!=card){
    val = (val*7) % 20201227;
    loop++;
}

val = 1;

for(int i=0;i<loop;i++){
    val = (val*door) % 20201227;
}

Console.WriteLine(val);