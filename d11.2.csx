using System.IO;
using System.Linq;

var initialData = File.ReadAllLines("d11.txt").Select(item=>item.ToCharArray()).ToArray();


bool unstable = true;
char[][] data = RoundPlay(initialData, ref unstable);


while(unstable)
{
    data = RoundPlay(data, ref unstable);
}
Console.WriteLine("------ "+data.SelectMany(x=>x).Count(crit=>crit == '#'));


void Print(char[][] input){
    foreach(var cArray in input)
    {
        Console.WriteLine(cArray);
    }
    Console.WriteLine("---------------------");
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
                    if(SeatCount(data, i, j) >= 5){
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
    for(int xScale= -1;xScale<=1;xScale++)
    {
        for(int yScale = -1;yScale<=1;yScale++)
        {
            if(xScale == 0 && yScale == 0) continue;
            if(OccupiedInADirection(data, x, y, xScale, yScale)){
                occupied++;
            }
        }
    }
    return occupied;
}

bool OccupiedInADirection(char[][] data, int initialX, int initialY, int xScale, int yScale)
{
    bool valid = true;
    int currentX = initialX+xScale;
    int currentY = initialY+yScale;
    while(valid)
    {
        valid = TryGetSeat(data, currentX, currentY, out char result);
        switch(result){
            case '#':
                return true;
            case 'L':
                return false;
        }
        currentX += xScale;
        currentY += yScale;
    }
    return false;
}

//note: x is NOT real x and y is NOT real y.
bool TryGetSeat(char[][] data, int x, int y, out char result)
{
    if(x < 0 || y < 0 || x >= data.Length || y >= data[x].Length){
        result = '\0';
        return false;
    }
    else
    {
        result = data[x][y];
    }
    return true;
}