var a=1;
var b=106700;
var c=123700;
var d=0;
var e=0;
bool f=false;
var g=0;
var h=0;

do{
  f = true;
  int half = (b/2)+1;
  for(d=2;d<half;d++){
   int half2=(b/d)+1;
   for(e=2;e<half;e++)
   {
      if(d*e==b){
         f=false;
         break;
      }
   }
   if(!f){
         break;
   }
  }
//if b is NOT prime number h++;
  if(!f){
    h++;
  }
  g=b-c;
  if(b-c!=0){
    b += 17;
  }
}while(g!=0);

Console.WriteLine(h);