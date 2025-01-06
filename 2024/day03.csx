char[] comparer = new char[]{'m','u','l','('};
char[] comparerDo = new char[]{'d','o','(',')'};
char[] comparerDont = new char[]{'d','o','n','\'','t','(',')'};

using (var file = File.OpenRead("day03.txt"))
using (var reader = new BinaryReader(file))
{
    long result = 0;
    bool enabled = true;
    do
    {
        switch(strGetState(reader))
        {
            case 0:
            if (enabled)
            {
                (bool valid, int num1) = readNumber(reader, ',');
                if (valid)
                {
                    (valid, int num2) = readNumber(reader, ')');
                    if (valid)
                    {
                        result += (long)(num1 * num2);
                    }
                }
            }
                break;
            case 1:
                enabled = true;
                break;
            case 2:
                enabled = false;
                break;
            default:
                break;
        }
    } while (file.Position < file.Length);
    Console.WriteLine(result);
}
bool strcmp(char[] str, BinaryReader reader, int startIndex = 0)
{
    int size = str.Length;
    for (int i = startIndex; i < size; i++)
    {
        char c = (char)reader.ReadByte();
        if (c != str[i]) return false;
        if (reader.BaseStream.Position >= reader.BaseStream.Length-1)
        {
            return false;
        }
    }
    return true;
}
int strGetState( BinaryReader reader)
{
    char c = (char)reader.ReadByte();
    switch(c)
    {
        case 'm':
            return strcmp(comparer, reader, 1)?0:-1;
        case 'd':
            if(strcmp(comparerDo, reader, 1))
            {
                return 1;
            }
            reader.BaseStream.Position--;
            if(strcmp(comparerDont, reader, 2))
            {
                return 2;
            }
            return -1;
        default:
            return -1;
    }
}

(bool valid, int num) readNumber(BinaryReader reader, char nextCharacter)
{
    long hexNumber = 0;
    int index = 0;
    bool validNumber = false;
    char c;
    do
    {
        c = (char)reader.ReadByte();
        if (reader.BaseStream.Position >= reader.BaseStream.Length)
        {
            return (false, 0);
        }
        if (c >= 0x30 && c <= 0x39)
        {
            validNumber = true;
            hexNumber += (c - 0x30) << (index * 4);
            index++;
        }
    }
    while (c >= 0x30 && c <= 0x39);
    if (validNumber && c == nextCharacter)
    {
        int finalNumber = 0;
        int decimalPosition = 1;
        index--;
        while (index >= 0)
        {
            int bitShifter = index * 4;
            finalNumber += (int)((hexNumber & (0xf<<bitShifter))>>bitShifter) * decimalPosition;
            decimalPosition *= 10;
            index--;
        }
        return (true, finalNumber);
    }
    return (false, 0);
}