using System.IO;
using System.Linq;

var banks = File.ReadAllText("day6.txt").Split(' ').Select(item=>int.Parse(item)).ToArray();
List<int[]> previous = new();

while(!previous.Any(item=>item.SequenceEqual(banks))){
    int[] arr = new int[banks.Length];
    banks.CopyTo(arr, 0);
    previous.Add(arr);
    int max = banks.Max();
    int index = Array.IndexOf(banks, max);
    banks[index] = 0;
    int i = (index+1)%banks.Length;
    while(max>0){
        banks[i]++;
        max--;
        i = (i+1)%banks.Length;
    }
}
Console.WriteLine(previous.Count);

var index = previous.FindIndex(item=>item.SequenceEqual(banks));
Console.WriteLine(previous.Count - index);