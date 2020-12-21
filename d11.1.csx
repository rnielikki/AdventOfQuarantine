using System.IO;
using System.Linq;

var initialData = File.ReadAllLines("d11.txt").Select(item=>item.ToCharArray()).ToArray();


char[][] data = initialData;
bool unstable = true;
//Print(one);
while(unstable)
{
    data = RoundPlay(data, ref unstable);
}
Print(data);
Console.WriteLine("------ "+data.SelectMany(x=>x).Count(crit=>crit == '#'));


void Print(char[][] input){
    foreach(var cArray in input)
    {
        Console.WriteLine(cArray);
    }
}

char[][] RoundPlay(char[][] data, ref bool edited)
{
    bool _edited = false;
    char[][] result = new char[data.Length][];
    for(int i=0;i<data.Length;i++)
    {
        char[] line = new char[data[i].Length];
        data[i].CopyTo(line, 0);
        for(int j=0;j<line.Length;j++)
        {
            switch(data[i][j])
            {
                case 'L':
                    if(SeatCount(data, i, j) == 0){
                        line[j]='#';
                        _edited = true;
                    }
                    break;
                case '#':
                    if(SeatCount(data, i, j) >= 4){
                        line[j] = 'L';
                        _edited = true;
                    }
                    break;
            }
        }
        result[i] = line;
    }
    edited = _edited;
    return result;
}
int SeatCount(char[][] data, int x, int y)
{
    int occupied = 0;
    for(int currentX = x-1;currentX<=x+1;currentX++)
    {
        for(int currentY = y-1;currentY<=y+1;currentY++)
        {
            if(currentX == x && currentY == y) continue;
            if(IsOccupied(data, currentX, currentY))
            {
                occupied++;
            }
        }
    }
    return occupied;
}

//note: x is NOT real x and y is NOT real y.
bool IsOccupied(char[][] data, int x, int y)
{
    if(x < 0 || y < 0 || x >= data.Length || y >= data[x].Length || data[x][y]!='#') return false;
    else return true;
}