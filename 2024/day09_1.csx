var data = File.ReadAllLines("day09.txt")[0].ToCharArray().Where(c => c>=0x30 && c<0x3a).Select(c => (byte)(c - 0x30)).ToArray();

var array =  MakeArray(data);
ReArrange(array);
//Print(array);
Console.WriteLine(GetCheckSum(array));

short[] MakeArray(byte[] input)
{
    int size = 0;
    foreach(var num in input)
    {
        size+=num;
    }
    var result = new short[size];

    bool isData = true;
    short data = 0;
    int index = 0;
    for(int i=0;i<input.Length;i++)
    {
        short value = -1;
        byte amount = input[i];
        if(isData)
        {
            value = data;
            data++;
        }
        for(int j=0;j<amount;j++)
        {
            result[index] = value;
            index++;
        }
        isData=!isData;
    }
    //Assure part
    if(index!=size) throw new InvalidOperationException("wtf help");
    return result;
}

void ReArrange(short[] input)
{
    for(int i=0;i<input.Length;i++)
    {
        if(input[i]>-1) continue;
        short value = -1;
        int position = input.Length;
        while(value < 0)
        {
            value = input[--position];
            if(i==position) return;
        }
        input[i] = value;
        input[position] = -1;
    }
}

ulong GetCheckSum(short[] input)
{
    ulong result = 0;
    for(int i=0;i<input.Length;i++)
    {
        var value = input[i];
        if(value<0) return result;
        result+=(ulong)(value*i);
    }
    throw new ArgumentException();
}

void Print(short[] input)
{
    foreach(var c in input)
    {
        Console.Write((c<0?'.':c)+ " ");
    }
    Console.WriteLine();
}
