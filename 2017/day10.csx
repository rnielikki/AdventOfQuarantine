using System.IO;
var input = File.ReadAllText("day10.txt");
var vals = input.Split(',').Select(val=>int.Parse(val));

int SIZE = 256;

int[] CreateHash(IEnumerable<int> values, int[] arr){
    foreach(int value in values){
        for(int index=0;index<value/2;index++){
            Swap(ref arr[(index+cursor)%SIZE], ref arr[(cursor+value-index-1)%SIZE]);
        }
        cursor = (cursor+value+skipSize)%SIZE;
        skipSize++;
    }
    return arr;
}
int cursor = 0;
int skipSize = 0;
int[] arr = Enumerable.Range(0,SIZE).ToArray();
var task1 = CreateHash(vals, arr);
Console.WriteLine(task1[0]*task1[1]);

var asciis = input.ToCharArray().Select(item=>(int)item).Concat(new int[]{17, 31, 73, 47, 23});
//var asciis = new int[]{17, 31, 73, 47, 23};

arr = Enumerable.Range(0,SIZE).ToArray();
cursor = 0;
skipSize = 0;
for(int i=0;i<64;i++){
    arr = CreateHash(asciis, arr);
}

int arrIndex = 0;
var c = arr.Take(16);
while(c.Any()){
    Console.Write(
        Convert.ToString(c.Aggregate((acc,val)=>acc^val),16)
    );
    arrIndex+=16;
    c = arr.Skip(arrIndex).Take(16);
}

/*
int arrIndex = 0;
for(int i=0;i<64;i++){
}
var c = circle.Take(16);
while(c.Any()){
    Console.Write(
        Convert.ToString(c.Aggregate((acc,val)=>acc^val),16)
    );
    arrIndex+=16;
    c = circle.Skip(arrIndex).Take(16);
}*/
/*
foreach(var s in circle)
     Console.Write(s+"   ");
Console.WriteLine("\nSkipSize "+ skipSize+"   "+cursor);
*/

void Swap<T>(ref T x, ref T y){
    T temp = x;
    x = y;
    y = temp;
}

//20056