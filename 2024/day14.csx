
int width = 101;
int height = 103;
var centerX = (int)(width / 2);
var centerY = (int)(height / 2);

var rgx = @"[\-]?[0-9]+";
var data = File.ReadAllLines("day14.txt").Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => {
    var r = System.Text.RegularExpressions.Regex.Matches(s, rgx).Select(s => s.Value).ToArray();
    return new RobotInfo(){
        P =  new Coord(){ X=int.Parse(r[0]), Y= int.Parse(r[1]) },
        V = new Coord(){ X= int.Parse(r[2]), Y= int.Parse(r[3]) }
    };
});
byte[,] arr = new byte[width,height];
const int streakRequired = 8;
for(int i=0;i<width*height;i++)
{
    if(IsStreak(simulate(i)))
    {
        Console.WriteLine(i);
    }
}
bool IsStreak(byte[,] arr)
{
    for(int y=0;y<height;y++)
    {
        int streak = 0;
        for(int x=0;x<width;x++)
        {
            var current = arr[x,y];
            if(current!=0)
            {
                streak++;
                if(streak>=streakRequired) return true;
            }
            else streak=0;
        }
    }
    return false;
}
byte[,] simulate(int movingAmount)
{
    flushArr();
    foreach(var robot in data)
    {
        int posx = (robot.V.X * movingAmount + robot.P.X) % width;
        int posy = (robot.V.Y * movingAmount + robot.P.Y) % height;
        if (posx < 0) posx += width;
        if (posy < 0) posy += height;
        arr[posx,posy]++;
    }
    return arr;
}
void flushArr()
{
    for(int j=0;j<height;j++) {
        for(int i=0;i<width;i++) {
            arr[i,j] = 0;
        }
    }
}
void MakeBitmap(byte[,] arr)
{
    for(int j=0;j<height;j++) {
        for(int i=0;i<width;i++) {
            var d = arr[i,j];
            Console.Write((d==0?'.':d) + " ");
        }
        Console.WriteLine();
    }
}

struct RobotInfo 
{
    public Coord P {get;set;}
    public Coord V {get;set;}

}

struct Coord
{
    public int X {get;set;}
    public int Y{get;set;}
}