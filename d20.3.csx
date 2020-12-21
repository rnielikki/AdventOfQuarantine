using System.IO;
using System.Linq;

List<string> stdInput = new();
string s;
while ((s = Console.ReadLine()) != null)
{
    stdInput.Add(s);
}
var data = stdInput.AsEnumerable().Select(a=>a.Select(b=>(b=='#')?true:false).ToArray()).ToArray();
var monster = File.ReadAllLines("d20.monster.txt").AsEnumerable().Select(a=>a.Select(b=>(b=='#')?true:false).ToArray()).ToArray();

var FullSize = data.Length;
List<(int, int)> monsterPath = new();

for(int y=0;y<monster.Length;y++){
    for(int x=0;x<monster[0].Length;x++){
        if(monster[y][x]){
            monsterPath.Add((x,y));
            //Console.WriteLine($"x: {x}, y: {y}");
        }
    }
}

var current = data;
bool done = false;
int tryIndex = 0;
while(!done && tryIndex < 4){
    done = CompareArray(current);
    if(done){
        EndTask(current);
        break;
    }
    current = Flip(current, tryIndex%2==0);
    done = CompareArray(current);
    if(done){
        EndTask(current);
        break;
    }
    current = Rotate(current);
    tryIndex++;
}

void EndTask(bool[][] result){
    var done = result.Select(a=>a.Select(b=>b?'#':'.').ToArray()).ToArray();
    PaintArray(done, result);
    Console.WriteLine(done.SelectMany(c=>c).Count(c=>c=='#'));
    /*
    foreach(var txt in done){
        Console.WriteLine(String.Concat(txt));
    }*/
}

bool CompareArray(bool[][] currentData){  
    bool found = false;
    for(int y = 0;y < FullSize-monster.Length+1; y++){
        for(int x=0;x < FullSize-monster[0].Length+1; x++){
            if(Compare(currentData, x,y)){
                found = true;
            }
        }
    }
    return found;
}

bool PaintArray(char[][] currentData, bool[][] compareData){   
    bool found = false;
    for(int y = 0;y < FullSize-monster.Length+1; y++){
        for(int x=0;x < FullSize-monster[0].Length+1; x++){
            if(Compare(compareData, x,y)){
                Paint(currentData,x,y);
            }
        }
    }
    return found;
}

bool Compare(bool[][] currentData, int x, int y){
    foreach((int xOff, int yOff) in monsterPath){
        if(!currentData[y+yOff][x+xOff]) return false;
    }
    return true;
}

void Paint(char[][] dataChar, int x, int y){
    foreach((int xOff, int yOff) in monsterPath){
        dataChar[y+yOff][x+xOff] = 'O';
    }
}
/*
foreach(var txt in dataChar){
    Console.WriteLine(String.Concat(txt));
}*/

//I don't know how.
//https://www.geeksforgeeks.org/rotate-a-matrix-by-90-degree-in-clockwise-direction-without-using-any-extra-space/
bool[][] Rotate(bool[][] pieceRaw){
        //https://stackoverflow.com/questions/15725840/copy-one-2d-array-to-another-2d-array   
        bool[][] piece = pieceRaw.Select(item=>item.ToArray()).ToArray();
        for (int i = 0; i < FullSize/2; i++)
        {
            for (int j = i; j < FullSize - 1 - i ; j++){
                bool temp = piece[i][j];
                piece[i][j] = piece[FullSize -1 - j][i];
                piece[FullSize -1 - j][i] = piece[FullSize -1 - i][FullSize -1 - j];
                piece[FullSize -1 - i][FullSize -1 - j] = piece[j][FullSize -1 - i];
                piece[j][FullSize -1 - i] = temp;
            }
        }
        return piece;
}

bool[][] Flip(bool[][] input, bool horizontal){
    if(horizontal){
        for(int x=0;x<FullSize;x++){
            input[x] = input[x].Reverse().ToArray();
        }
        return input;
    }
    else{
        for(int x=0;x<FullSize/2;x++){
            Swap(ref input[x], ref input[FullSize-1-x]);
        }
        return input;
    }
}
void Swap<T>(ref T a, ref T b)
{
    var temp= a;
    a = b;
    b = temp;
}