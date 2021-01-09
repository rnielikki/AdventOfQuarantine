class App{
    static readonly string[] program = File.ReadAllLines("day18.txt");
    Dictionary<char,long> variables = new Dictionary<char, long>();
    long currentLine = 0;
    Queue<long> queue = new();
    public bool retrying { get; private set; } = false;
    public bool end { get; private set; } = false;
   
    public int sent { get; private set;}

    private App another;
    public App(int programId){ variables.Add('p', programId);}
    public App(int programId, App app):this(programId){ another = app; app.another = this; }
    public void Run(){
        if(currentLine >= program.Length){ end = true; return; }
        if(end) return;
        RunInternal();
        currentLine++;
    }
    private void RunInternal(){
        var cut = program[(int)currentLine].Split(' ');
        var target = cut[1];
        var value = 0L;
        if(cut.Length>2){
            value = getVar(cut[2]);
        }
        switch(cut[0]){
            case "set":
                setVar(target[0], value);
                break;
            case "add":
                setVar(target[0], getVar(target)+value);
                break;
            case "mul":
                setVar(target[0], getVar(target)*value);
                break;
            case "mod":
                setVar(target[0], getVar(target)%value);
                break;
            case "snd":
                another.queue.Enqueue(getVar(target));
                sent++;
                break;
            case "rcv":
                if(queue.Count == 0){
                    retrying = true;
                    if(another.retrying || another.end){
                        end = true;
                        another.end = true;
                        return;
                    }
                    else{
                        currentLine--;
                    }
                }
                else{
                    retrying = false;
                    setVar(target[0], queue.Dequeue());
                }
                break;
            case "jgz":
                if(getVar(cut[1])>0L)
                    currentLine += value-1;
                break;
            default:
                throw new FormatException("file format wrong");
        }
    }
    private void setVar(char variable, long value){
        if(!variables.ContainsKey(variable)){
            variables.Add(variable, value);
        }
        else{
            variables[variable] = value;
        }
    }
    private long getVar(string value){
        if(!long.TryParse(value, out long tryGet)){
            if(!variables.ContainsKey(value[0])){
                variables.Add(value[0],0);
                return 0;
            }
            else{
                return variables[value[0]];
            }
        }
        else{
            return tryGet;
        }
    }
}
App app1= new App(0);
App app2 = new App(1, app1);


while(!app1.end || !app2.end){
    app1.Run();
    app2.Run();
}
Console.WriteLine(app2.sent);