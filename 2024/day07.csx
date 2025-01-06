const string fileName = "day07.txt";
var data = File.ReadAllLines(fileName).Select(a => {
    int delimeter = a.IndexOf(':');
    return new LineData(){
        result = long.Parse(a.Substring(0, delimeter)),
        values = a.Substring(delimeter+2).Trim().Split(" ").Select(b => int.Parse(b)).ToArray()
    };
});

long res = 0;
foreach(var line in data)
{
    if(TestOperator(line, 0, line.values[0]))
    {
        res+=line.result;
    }
}
Console.WriteLine(res);

bool TestOperator(LineData input, int startIndex, long currentValue)
{
    bool valid = false;
    var values = input.values;
    int nextIndex = startIndex + 1;
    //too big value, invalid, no point to continue
    if(currentValue > input.result) return false;
    if(nextIndex==values.Length)
    {
        return currentValue==input.result;
    }
    long nextValue = (long)values[nextIndex];
    //test plus
    valid|=TestOperator(input, nextIndex, checked(currentValue+nextValue));
    //test mul
    valid|=TestOperator(input, nextIndex, checked(currentValue*nextValue));
    //test concat
    valid|=TestOperator(input, nextIndex, checked(currentValue*GetDigitMultiplier(values[nextIndex]))+nextValue);
    return valid;
}
long GetDigitMultiplier(int input)
{
    long current = input;
    long result = 1;
    while(current>0)
    {
        current/=10;
        result*=10;
    }
    return result;
}


public struct LineData
{
    public int[] values { get; init; }
    public long result { get; init; }
}
