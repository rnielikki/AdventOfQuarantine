//CONST
long AFactor = 16807;
long BFactor = 48271;
long Divider = int.MaxValue;
long Mask = ushort.MaxValue;

//long FirstA = 65;
//long FirstB = 8921;
long FirstA = 289;
long FirstB = 629;

//CONST END
//VARS
long A = FirstA;
long B = FirstB;
int count = 0;
//VARS END
//MATCH ONE
int Repeat = 40_000_000;

for(int i=0;i<Repeat;i++){
    A = (A*AFactor)%Divider;
    B = (B*BFactor)%Divider;
    if((Mask & A ) == (Mask & B)){ count++; }
}
Console.WriteLine(count);

//MATCH TWO
A = FirstA;
B = FirstB;
Repeat = 5_000_000;
count = 0;
for(int i=0;i<Repeat;i++){
    do{
        A = (A*AFactor)%Divider;
    }while(A%4!=0);
    do{
        B = (B*BFactor)%Divider;
    }while(B%8!=0);
    if((Mask & A ) == (Mask & B)){ count++; }
}
Console.WriteLine(count)