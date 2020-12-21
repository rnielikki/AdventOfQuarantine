using System.IO;
using System.Linq;

var reader = new StreamReader("d20.txt");

Dictionary<int, bool[][]> parsed = new();

while(!reader.EndOfStream){
    string txt = reader.ReadLine();
    if(txt!=""){
        if(txt[0]=='T'){
            int tileNumber = int.Parse(
                string.Concat(
                    txt.AsEnumerable()
                    .SkipWhile(c=>!char.IsDigit(c))
                    .TakeWhile(c=>char.IsDigit(c))
                )
            );
            txt = reader.ReadLine();
            List<bool[]> array = new();
            while(txt!="" && !reader.EndOfStream){
                array.Add(txt.Select(c=>c=='#'?true:false).ToArray());
                txt = reader.ReadLine();
            }
            if(txt!=""){
                array.Add(txt.Select(c=>c=='#'?true:false).ToArray());
            }

            var res = array.ToArray();
            parsed.Add(
                tileNumber,
                new bool[][]
                {
                    res[0], res.Select(item=>item[0]).ToArray(),
                    res[^1], res.Select(item=>item[^1]).ToArray()
                }
            );
        }
    }
}
long acc = 1;
List<bool[]> values = parsed.SelectMany(p=>
        p.Value.Concat(
            p.Value.Select(
                v=>v.Reverse().ToArray()
            ).ToArray()
        )
    ).ToList();
foreach(var box in parsed){
    int v = box.Value.Count(line=>
        values.Count(v=>v.SequenceEqual(line)) <= 1
    );
    //Console.WriteLine(box.Key+" :: "+v);
    if(v>=2){ acc*=box.Key;}
}
Console.WriteLine(Math.Sqrt(parsed.Count));
Console.WriteLine(acc);