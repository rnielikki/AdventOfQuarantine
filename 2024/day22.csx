var data = File.ReadAllLines("day22.txt");
//long sum;
Dictionary<int, int> everyData = new();

foreach(var line in data)
{
    if(!string.IsNullOrWhiteSpace(line))
    {
        Dictionary<int,int> dic = new();
        var secret = int.Parse(line);
        //int[] ref = new int[4];
        //var secret2 = ;
        int before = secret%10;
        int current = before;
        int series = 0;
        for(int i=0;i<2000;i++)
        {
            secret = Perform(secret);
            current = secret%10;
            var change = current-before;
            before = current;
            series = Push(series, change);
            if(i>=3)
            {
                if(!dic.ContainsKey(series)) dic.Add(series, current);
            }
        }
        foreach(var (k, v) in dic)
        {
            if(everyData.ContainsKey(k)) everyData[k]+=v;
            else everyData.Add(k, v);
        }
    }
}
Console.WriteLine(everyData.Values.Max());

int Push(int key, int data)
{
    key <<=5;
    key&= 1048575;
    key+=data+10;
    return key;
}

int Perform(int input)
{
    //line 1---------------------
    long num = input<<6;
    num^= input;        //mix
    num &= 16777215;    //prune
    //line 2---------------------
    long num2 = num;
    num2>>=5;
    num2^= num;        //mix
    num2 &= 16777215;    //prune
    //line 3---------------------
    long num3 = num2;
    num3<<=11;
    num3^= num2;        //mix
    num3 &= 16777215;    //prune
    return (int)num3;
}