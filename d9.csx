using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

const int range = 25;

//--------------------------- 1 ::: 21806024

/*
using (var dataStream = File.OpenRead("d9.txt"))
using (var reader = new StreamReader(dataStream))
{
    Console.WriteLine(FindErrorNumber(reader));
}
*/

//--------------------------- 2
long errorNumber;
using (var dataStream = File.OpenRead("d9.txt"))
using (var reader = new StreamReader(dataStream))
{
    errorNumber = FindErrorNumber(reader);
}
var results = FindNumberSeries(errorNumber);
Console.WriteLine(results.Min() + results.Max());

IEnumerable<long> FindNumberSeries(long targetNumber)
{
    //easy simple meh way
    long[] series = File.ReadAllLines("d9.txt").Select(item => long.Parse(item)).ToArray();
    for (int i = 0; i < series.Length; i++)
    {
        long sum = 0;
        for (int j = i; j < series.Length; j++)
        {
            sum += series[j];
            if (sum == targetNumber)
            {
                return series.Skip(i).Take(j - i);
            }
            else if(sum > targetNumber)
            {
                break;
            }
        }
    }
    return Enumerable.Empty<long>();
}

//--------------------------- common
long FindErrorNumber(StreamReader streamReader)
{
    string text = streamReader.ReadLine();
    Queue<long> queue = new();
    do
    {
        long target = long.Parse(text);
        if (queue.Count >= range)
        {
            var array = queue.ToArray();
            bool found = false;
            for (int i = 0; i < range; i++)
            {
                for (int j = i+1; j < range; j++)
                {
                    if (i == j) continue;
                    if (array[i] + array[j] == target) {
                        found = true;
                    }
                }
            }
            if (!found) return target;
            queue.Dequeue();
        }
        queue.Enqueue(target);
        text = streamReader.ReadLine();
    } while (text != null);
    return -1; //should not reach
}
