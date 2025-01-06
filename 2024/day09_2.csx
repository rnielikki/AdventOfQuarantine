var data = File.ReadAllLines("day09.txt")[0].ToCharArray().Where(c => c>=0x30 && c<0x3a).Select(c => (byte)(c - 0x30)).ToArray();

(var array, var emptyArray) =  MakeArray(data);
var newArr = ReArrange(array, emptyArray);
Console.WriteLine(GetCheckSum(newArr, emptyArray));


(DataInfo[] array, DataInfo[] emptyArray) MakeArray(byte[] input)
{
    var emptyResult = new DataInfo[input.Length/2];
    var result = new DataInfo[input.Length - emptyResult.Length];

    bool isData = true;
    int data = 0;
    int resultIndex = 0;
    int emptyResultIndex = 0;
    int index = 0;
    for(int i=0;i<input.Length;i++)
    {
        int value = -1;
        byte length = input[i];
        if(isData)
        {
            value = data;
            data++;
            result[resultIndex] = new DataInfo()
            {
                Data = value,
                Index = index,
                Length = length
            };
            resultIndex++;
        }
        else
        {
            emptyResult[emptyResultIndex] = new DataInfo()
            {
                Data = -1,
                Index = index,
                Length = length
            };
            emptyResultIndex++;
        }
        isData=!isData;
        index+=length;
    }
    return (result, emptyResult);
}
DataInfo[] ReArrange(DataInfo[] input, DataInfo[] emptyInput)
{
    List<DataInfo> newInput = new();
    for(int i=0;i<input.Length;i++)
    {
        var info = input[input.Length-i-1];
        for(int j=0;j<emptyInput.Length;j++)
        {
            var space = emptyInput[j];
            //no point to go further
            if(info.Index <= space.Index) break;
            //found good space
            if(space.Length >= info.Length)
            {
                var newData = info;
                newData.Index = space.Index;
                newInput.Add(newData);
                space.Length -= info.Length;
                space.Index += info.Length;
                //fuck I defined as struct loll
                emptyInput[j] = space;
                input[input.Length-i-1].Data = -1;
                break;
            }
        }
    }
    return input.Union(newInput).ToArray();
}
ulong GetCheckSum(DataInfo[] input1, DataInfo[] input2) => GetCheckSum(input1) + GetCheckSum(input2);
ulong GetCheckSum(DataInfo[] input)
{
    ulong result = 0;
    foreach(var info in input)
    {
        for(int i=info.Index;i<info.Index + info.Length;i++)
        {
            var value = info.Data;
            if(value<0) continue;
            result+=(ulong)(value*i);
        }
    }
    return result;
}

void Print(DataInfo[] dataInfo, DataInfo[] emptyInfo)
{
    foreach(var info in dataInfo.Union(emptyInfo).OrderBy(d => d.Index))
    {
        for(int i=0;i<info.Length;i++)
        {
            Console.Write((info.IsEmpty()?'.':info.Data)+ " ");
        }
    }
    Console.WriteLine();
}

public struct DataInfo
{
    public int Data { get; set; }
    public int Index { get; set; }
    public byte Length { get; set; }
    public bool IsEmpty() => Data < 0;
}