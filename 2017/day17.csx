List<int> Numbers = new(){0};

//________ FIRST
int Last = 2017;
int Skip = 337;
//vars
int Cursor = 0;

for(int i=1;i<=Last;i++){
    //Console.Write(Cursor+"... ");
    Cursor = (Cursor+Skip+1)%Numbers.Count;
    Numbers.Insert(Cursor+1,i);
}
Console.WriteLine(Numbers[Cursor+2]);
//_________ SECOND
Last = 50000000;
Cursor = 0;
Numbers.Clear();
Numbers.Add(0);
for(int i=1;i<=Last;i++){
    //Console.Write(Cursor+"... ");
    Cursor = (Cursor+Skip+1)%Numbers.Count;
    if(Cursor==0)
        Numbers.Insert(Cursor+1,i);
    else Numbers.Add(i);
}
Console.WriteLine(Numbers[1]);