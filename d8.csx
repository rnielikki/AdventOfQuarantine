using System.IO;
using System.Collections.Generic;

var lines = File.ReadAllLines("d8.txt");

//--------------------------------- 1 ::: 1586
TryCheck(lines, out int result, out _);
Console.WriteLine("accumulator " + result);
//--------------------------------- 2 ::: 703

TryCheck(lines, out _, out Dictionary<int, (string, int)> nopJmp); //get affected only line numbers with nop/jmp
foreach (int i in nopJmp.Keys)
{
    var (str, num) = nopJmp[i];
    switch(str)
    {
        case "jmp":
            Test(i, str, num, "nop");
            break;
        case "nop":
            Test(i, str, num, "jmp");
            break;
    }
}

void Test(int currentLine, string str, int value, string replacement)
{
    string[] strs = new string[lines.Length];
    lines.CopyTo(strs, 0);
    strs[currentLine] = $"{replacement} {value}";
    if (TryCheck(strs, out int acc, out _)) {
        Console.WriteLine("the acc is "+acc);
    }
}
//--------------------------------- common
bool TryCheck(string[] data, out int accumulator, out Dictionary<int, (string, int)> nopJmp)
{
    accumulator = 0;
    HashSet<int> lineNumbers = new();
    nopJmp = new();
    int currentLine = 0;
    bool isInRange = true;
    while ( isInRange && !lineNumbers.Contains(currentLine))
    {
        lineNumbers.Add(currentLine);
        var (command, value) = Parse(data[currentLine]);
        switch (command) {
            case "nop":
                nopJmp.Add(currentLine, (command, value));
                currentLine++;
                break;
            case "acc":
                accumulator += value;
                currentLine++;
                break;
            case "jmp":
                nopJmp.Add(currentLine, (command, value));
                currentLine += value;
                break;
        }
        isInRange = (currentLine > 0 && currentLine < data.Length);
    }
    return !isInRange;
}


(string, int) Parse(string input)
{
    var s = input.Split(" ");
    return (s[0], int.Parse(s[1]));
}
