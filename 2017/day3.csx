int input = 265149;
int arr_size = 15;
int arr_size_half = arr_size/2;
int[,] arr = new int[arr_size, arr_size];

arr[arr_size_half, arr_size_half] = 1;
int y;
int x = y = arr_size_half;
int scale = 1;
int offset = 0;
int current = 1;
direction dir = new direction(1,0);

int currentNum = 0;

while(arr[x,y] < input){
    bool turn = false;
    if(current%scale==0){
        current = 0;
        if(offset ==0){
            offset++;
        }
        else{
            scale++;
            offset = 0;
        }
        turn = true;
    }
    x+=dir.X;
    y+=dir.Y;
    //if(arr[x,y]!=0) throw new InvalidOperationException("overwriting "+arr[x,y]);
    currentNum = sumAround(x,y);
    arr[x,y]=currentNum;
    current++;
    if (turn) dir.turn();
}
//print_array(arr);
Console.WriteLine(currentNum);

int sumAround(int x, int y){
    int sum = 0;
    for(int i=-1;i<=1;i++){
        for(int j=-1;j<=1;j++){
            sum+=arr[x+i,y+j];
        }
    }
    return sum;
}

void print_array(int[,] arr){
    for(int i=0;i<arr.GetLength(0);i++){
        for(int j=0;j<arr.GetLength(1);j++){
            Console.Write(" "+arr[i,j]+" ");
        }
        Console.WriteLine();
    }
}

class direction{
    public int X = 0; public int Y = 0;
    public direction(int x, int y) {X = x; Y = y;}
    public void turn(){
        switch(X){
            case -1:
                X = 0; Y = 1;
                break;
            case 0:
                if(Y==1)
                {
                    X= 1; Y = 0;
                }
                else
                {
                    X =-1; Y = 0;
                }
                break;
            case 1:
                X = 0; Y = -1;
                break;
        }
    }
}