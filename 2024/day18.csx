#load  "./_libdijkstra.csx"

const int Size = 71;

var coords = File.ReadAllLines("day18.txt").Select(s => s.Split(',')).ToArray();
var data = new bool[Size,Size];

for(int i=0;i<1024;i++) {
    UpdateCoord(coords[i]);
}
 var startIndex = coords.Length - 1024;
 var indexAdder = startIndex>>2;
var lastPass = 1024;
var lastFail = coords.Length - 1;

var index = lastPass + (lastFail - lastPass) /2;

while(lastFail-lastPass > 1)
{
    for(int i=lastPass;i<=lastFail;i++) {
        UpdateCoord(coords[i],i<=index);
    }
    var index2 = index+1024;
    var result = (new PathFinder(data, new Coordinate(0,0), new Coordinate(70, 70))).Find();

    if(result.Paths.Count==0) {
        lastFail = index;
    }
    else {
        lastPass = index;
    }
    index = lastPass + (lastFail - lastPass) /2;
}
Console.WriteLine($"{lastFail} {coords[lastFail][0]},{coords[lastFail][1]}");

void UpdateCoord(string[] coordsIndex, bool isWall = true) {
    var x = int.Parse(coordsIndex[0]);
    var y = int.Parse(coordsIndex[1]);
    data[x,y] = isWall;
}