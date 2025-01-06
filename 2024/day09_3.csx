var data = File.ReadAllLines("day09_3.txt")[0].ToCharArray().Where(c => c>=0x30 && c<0x3a).Select(c => (byte)(c - 0x30)).ToArray();

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
    int i=0;
    while(i<input.Length)
    {
        int index = input.Length-i-1;
        var val = input[index];
        if(val == -1)
        {
            i++;
            continue;
        }
        var pos = index;
        int size = 0;
        while(input[pos]==val)
        {
            pos--;
            size++;
            if(pos<0) break;
        }
        pos++; //current position
        
        //find empty
        for(int j=0;j<index;j+=0)
        {
            var emptyVal = input[j];
            if(emptyVal< 0)
            {
                var len = getEmptyLength(j);
                if(len >= size)
                {
                    //found
                    for(int a=0;a<size;a++)
                    {
                        input[j+a] = val;
                        input[index-a]=-1;
                    }
                    break;
                }
                j+=len;
            }
            else j++;
        }
        i+=size;
    }
    int getEmptyLength(int index)
    {
        int res = 0;
        while(index+res < input.Length && input[index+res]<0)
        {
            res++;
        }
        return res;
    }
}

ulong GetCheckSum(short[] input)
{
    ulong result = 0;
    for(int i=0;i<input.Length;i++)
    {
        var value = input[i];
        if(value<0) continue;
        result+=(ulong)(value*i);
    }
    return result;
}

void Print(short[] input)
{
    foreach(var c in input)
    {
        Console.Write((c<0?'.':c)+ " ");
    }
    Console.WriteLine();
}
