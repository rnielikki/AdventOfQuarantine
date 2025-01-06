int[] data = File.ReadAllLines("day11.txt")[0].Split(" ").Select(a => int.Parse(a.Trim())).ToArray();
const int maxDepth = 75;
const int sampleAmount = 40;
long[,] numberMap = new long[16,sampleAmount];

Dictionary<string,long> cache = new();

Console.WriteLine(await Blink(data, maxDepth));

async Task MakeSample()
{
    Task<long>[] sampleTasks = new Task<long>[10];
    for(int i=0;i<10;i++)
    {
        Console.WriteLine("add to sample collection "+i);
        var wtf = i;
        sampleTasks[i] = Task.Run(() => BlinkSample((long)wtf, sampleAmount, wtf));
    }
    Console.WriteLine("collecting sample...");
    await Task.WhenAll(sampleTasks);
}

async Task<long> Blink(int[] arr, int depth) {
   long count = 0;
   for(int i=0;i<data.Length;i++)
   {
       var wtf = i;
       count+=blink_cached((long)arr[wtf], depth);
   }
   return count;
}
long BlinkSample(long num, int depth, int original) {
   if(depth <= 0)
   {
      return 1;
   }
   numberMap[original,sampleAmount-depth]++;
   long count = 0;
   int length = GetLength(num);
   if(num==0) count+= BlinkSample(1, depth-1, original);
   else if((length & 1)==0) {
      var divider = Pow10((length/2)+1);

      count+=BlinkSample(num/divider, depth-1, original);
      count+=BlinkSample(num%divider, depth-1, original);
   }
   else {
       count+=BlinkSample(num*2024, depth-1, original);
   }
   return count;
}


long BlinkEach(long num, int depth) {
   if(depth <= 0)
   {
      return 1;
   }
   long count = 0;
   int length = GetLength(num);
   if(num==0) {
    count+=blink_cached(1, depth-1);
   } 
   else if((length & 1)==0) {
      var divider = Pow10((length/2)+1);
      count+=blink_cached(num/divider, depth-1);
      count+=blink_cached(num%divider, depth-1);
   }
   else {
      count+=blink_cached(num*2024, depth-1);
   }
   return count;
}

long blink_cached(long num, int depth) {
    if (cache.TryGetValue($"{num}_{depth}", out var res)) {
        return res;
    }
    res = BlinkEach(num, depth);
    cache.Add($"{num}_{depth}",res);

    return res;
}

int GetLength(long input)
{
    long current = input;
    int result = 0;
    while(current>0)
    {
        current/=10;
        result++;
    }
    return result;
}
long Pow10(int length)
{
    long res = 1;
    for(int i=1;i<length;i++) res*=10;
    return res;
}

void PrintSampleMap(long[,] map)
{
    for(int i=0;i<10;i++)
    {
        Console.Write(i+" :: ");
        for(int j=0;j<sampleAmount;j++)
        {
            Console.Write(numberMap[i,j]+"  ");
        }
        Console.WriteLine();
    }
}