using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

var textStream = File.Open("d6.txt", FileMode.Open);
StreamReader reader = new(textStream);
int asdf = 0;
string line = "";

do
{
    line = reader.ReadLine();
    var groupLine = line.AsEnumerable();
    //Console.WriteLine(line);

    while (groupLine!= null)
    {
        string anotherGroup = reader.ReadLine();
        if (!(anotherGroup is { Length: > 0 })) break;
        //Console.WriteLine(anotherGroup);
        groupLine = groupLine.Intersect(anotherGroup);
    }
    //Console.WriteLine(groupLine?.Count() ?? 0);
    asdf += groupLine?.Count() ?? 0;

} while (line != null);

Console.WriteLine(asdf);
