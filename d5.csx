using System.IO;
using System.Collections.Generic;
using System.Linq;

var fstream = File.OpenRead("d5.txt");
List<int> positions = new();
int seatId = 0;

int charCode = fstream.ReadByte();
while(charCode!=-1)
{
    char buf = (char)charCode;
    switch(buf)
    {
        case 'B':
        case 'R':
            seatId <<= 1;
            seatId += 1;
            break;
        case 'F':
        case 'L':
            seatId <<= 1;
            break;
        case '\n':
            positions.Add(seatId);
            seatId = 0;
            break;
    }
    charCode = fstream.ReadByte();
}
positions.Add(seatId);

IOrderedEnumerable<int> idSet = positions.OrderBy(x=>x);
int max = idSet.Last();

//------------------------------------------ 1
Console.WriteLine(max);

// --------------------------------------- 2
var mySeat = Enumerable.Range(idSet.First(), max).Except(idSet);
Console.WriteLine(mySeat.First());