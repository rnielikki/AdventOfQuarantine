using System.IO;
using System.Collections.Generic;
using System.Linq;

var data = new HashSet<Box>();
var floor1 = File.ReadAllLines("d17.txt")
    .Select(a=>a.ToCharArray().ToArray()).ToArray();

for (int i=0;i<floor1.Length;i++){
    for(int j=0;j<floor1.Length;j++){
        if(floor1[i][j]=='#'){
            data.Add(new Box(j,i,0));
        }
    }
}
SetMinMax(data);

HashSet<Box> res = data;
int x = 0;
while(x<6){
    res = ArrangeBoxes(res);
    x++;
}
Console.WriteLine(res.Count);

HashSet<Box> ArrangeBoxes (HashSet<Box> set){
    var boxes = new HashSet<Box>();
    for(int i=Box.YMin;i<=Box.YMax;i++){
        for(int j=Box.XMin;j<=Box.XMax;j++){
            for(int l=Box.ZMin;l<=Box.ZMax;l++){
                var box = new Box(j,i,l);
                switch(GetActiveCount(set, box)){
                    case 2:
                        if(set.Contains(box)){
                            boxes.Add(box);
                        }
                        break;
                    case 3:
                        boxes.Add(box);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    SetMinMax(boxes);
    return boxes;
}

byte GetActiveCount(HashSet<Box> set, Box box){
    byte count = 0 ;
    for(int i=box.Y-1;i<=box.Y+1;i++){
        for(int j=box.X-1;j<=box.X+1;j++){
            for(int l=box.Z-1;l<=box.Z+1;l++){
                if(!box.ExactSame(j,i,l) && set.Contains(new Box(j,i,l))){
                    count++;
                }
            }
        }
    }
    return count;
}

void SetMinMax(HashSet<Box> set){
    Box.SetMin(set.Min(b=>b.X), set.Min(b=>b.Y), set.Min(b=>b.Z));
    Box.SetMax(set.Max(b=>b.X), set.Max(b=>b.Y), set.Max(b=>b.Z));
}


//struct: a.Equals(b) is ok.
struct Box{
    public static int XMin { get; private set; }
    public static int YMin { get; private set; }
    public static int ZMin { get; private set; }

    public static int XMax { get; private set; }
    public static int YMax { get; private set; }
    public static int ZMax { get; private set; }

    public int X  { get; private set; }
    public int Y { get; private set; }
    public int Z { get; private set; }
    public Box (int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public static void SetMin ( int x, int y, int z )
    {
        XMin = x-1;
        YMin = y-1;
        ZMin = z-1;
    }
    public static void SetMax ( int x, int y, int z ){
        XMax = x+1;
        YMax = y+1;
        ZMax= z+1;
    }
    public bool ExactSame(int x, int y, int z){
        return (X == x && Y == y && Z == z);
    }
    public override string ToString()
    {
        return $"[{X}, {Y}, {Z}]";
    }
}