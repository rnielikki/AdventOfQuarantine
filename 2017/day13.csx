var data =
File.ReadAllLines("day13.txt").Select(item=>{
    var pair = item.Split(':');
    int key = int.Parse(pair[0]);
    return new KeyValuePair<int, Layer>(key, new Layer(key, int.Parse(pair[1])));
}).ToDictionary(kv=>kv.Key, kv=>kv.Value);
var layers = data.Values;

int caught = 0;
int cursor;
int MAX_DATA = data.Keys.Max()+1;

//-----------1

for(cursor=0;cursor<MAX_DATA;cursor++){
    if(data.ContainsKey(cursor)){
        var current = data[cursor];
        if(current.Catch()){
            caught += current.Capacity * cursor;
        }
    }
    foreach(var layer in layers){
        layer.MoveNext();
    }
}
Console.WriteLine(caught);

//---------------2
int skip = 0;
while(true){
    if(layers.All
        (layer=>
        (skip+layer.Label)%(layer.Capacity*2-2) != 0)
    ){
        break;
    }
    skip++;
}
Console.WriteLine(skip);

class Layer{
    public int Capacity { get; }
    public int Position { get; private set; }
    public int Label { get; }
    private bool _isForward = true;
    public Layer(int label, int capacity){
        Label = label;
        Capacity = capacity;
        Position = 0;
    }
    public void MoveNext(){
        if(_isForward){
            Position++;
            if(Position>=Capacity-1){
                _isForward = false;
            }
        }
        else{
            Position--;
            if(Position<=0){
                _isForward = true;
            }
        }
    }
    public bool Catch() => Position == 0;
}