var currentLine = 0;

var __lines = File.ReadAllLines("day23.txt");
var lines = __lines.Select(line=>{
    var split = line.Split(' ');
    var caseName = "\ncase_"+(currentLine+1)+":";
    string result;
    switch(split[0]){
        case "set":
            result=$"{split[1]} = {split[2]};";
            break;
        case "sub":
            result=$"{split[1]} -= {split[2]};";
            break;
        case "mul":
            result=$"{split[1]} *= {split[2]}; multiple++;";
            break;
        case "jnz":
            int lineOffset = int.Parse(split[2]);
            int lineJump = currentLine + lineOffset;
            if(lineJump >= __lines.Length){
                lineJump = __lines.Length;
            }
            if(char.IsDigit(split[1][0]) && split[1][0]!='0'){
                if(lineOffset == 2) result = "if("+split[1]+"==0)";
                else result = "goto case_"+lineJump+";";
            }
            else{
                result="if("+split[1]+"!=0){ goto case_"+lineJump+"; }";
            }
            break;
        default:
            throw new FormatException("input is wrong");
    }
    currentLine++;
    return result+caseName;
});

Console.WriteLine("/* generated from day23.pre.csx. Usage: 'dotnet day23.pre.csx > day23.csx' */");
for(int i=0;i<8;i++){
    Console.WriteLine("long "+(char)(97+i)+"= 0;");
}
Console.WriteLine("long multiple = 0;");
//for  part 2
//Console.WriteLine("a = 1;");
Console.WriteLine("case_0:");
foreach(var line in lines){
    Console.WriteLine(line);
}
Console.WriteLine("Console.WriteLine(multiple);");
for(int i=0;i<8;i++){
    Console.WriteLine("Console.Write("+(char)(97+i)+"+\"     \");");
}