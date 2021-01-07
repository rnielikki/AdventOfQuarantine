using System.IO;
using System.Linq;

Info info;

using (var reader = new StreamReader("day25.txt")){
    var first = reader.ReadLine()[^2];
    int max = int.Parse(
        new string(reader.ReadLine()
        .SkipWhile(c=>!char.IsDigit(c))
        .TakeWhile(c=>char.IsDigit(c)).ToArray())
    );
    info = new Info(first, max);

    while(!reader.EndOfStream){
        var line = reader.ReadLine().Trim();
        if(line is { Length:>3}){
            var instructions = new Instructions();
            info.AddInstruction(line[^2], instructions);
            line = reader.ReadLine().Trim();
            int currentCondition = -1;
            while(line is{ Length:>3 }){
                switch(line.Substring(0,3)){
                    case "In ":
                        throw new ArgumentException("Syntax error: missing line break");
                    case "If ":
                        currentCondition = int.Parse(line[^2].ToString());
                        break;
                    case "- W":
                        var currentNumber = int.Parse(line[^2].ToString());
                        Action action = (currentNumber==1)?info.Add:info.Remove;
                        instructions.AddCondition(action, currentCondition);
                        break;
                    case "- M":
                        string move = line.Substring(line.LastIndexOf(' ')+1);
                        switch(move){
                            case "left.":
                                instructions.AddCondition(info.MoveLeft, currentCondition);
                                break;
                            case "right.":
                                instructions.AddCondition(info.MoveRight, currentCondition);
                                break;
                            default:
                                throw new ArgumentException($"Unknown direction {move}. Direction should be either left or right.");
                        }
                        break;
                    case "- C":
                        var newTarget = line[^2];
                        instructions.AddCondition(()=>info.Call(newTarget), currentCondition);
                        break;
                }
                line = reader?.ReadLine()?.Trim();
            }
        }
    }
}
info.Start();
Console.WriteLine(info.GetCount());

class Instructions{
    List<Action>[] Actions = new List<Action>[2]{ new(), new() };
    public void AddCondition(Action action, int index){
        Actions[index].Add(action);
    }
    public void CallAll(Info info){
        foreach(var a in Actions[info.IsOne()?1:0]){
            a();
        }
    }
}
class Info{
    static Dictionary<char, Instructions> Actions = new();
    public int Cursor { get; set; } = 0;
    HashSet<int> Tape = new();
    
    public char First { get; init;}
    public int MAX_STEPS { get; init; }
    int currentStep = 0;
    public Info(char first, int maxStep){
        MAX_STEPS = maxStep;
        First = first;
    }
    public void AddInstruction(char name, Instructions instructions){
        Actions.Add(name, instructions);
    }
    public int GetCount() => Tape.Count;
    public bool IsOne()=>Tape.Contains(Cursor);
    //---
    public void Add()=>Tape.Add(Cursor);
    public void Remove()=>Tape.Remove(Cursor);
    public void MoveLeft() => Cursor--;
    public void MoveRight() => Cursor++;
    public void Call(char name){
        currentStep++;
        if(currentStep > MAX_STEPS) return;
        else Actions[name].CallAll(this);
    }
    public void Start(){
        Call(First);
    }
}
/*
Begin in state A.
Perform a diagnostic checksum after 6 steps.

In state A:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state B.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the left.
    - Continue with state B.
*/