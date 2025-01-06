//--------------- NOTE HERE
// 1. You need manual input in Logic() function
// 2. If you are UNLUCKY with the input, this code might NOT work, like this example >:(

int[] program = new int[]{2,4,1,2,7,5,1,3,4,3,5,5,0,3,3,0};
//int[] program = new int[]{2,4,1,2,7,5,0,3,4,7,1,7,5,5,3,0};

ulong inputA = 0;
ulong inputMax = 44;
var outputIndex = 0;
var valid = false;
var runCounter = 0;

Array.Reverse(program);
/*
foreach(var something in program)
{
   A<<=3;
   bool found = false;
   for(int i=0;i<8;i++)
   {
      var result = Logic(A+Convert.ToUInt64(i));
      if(result==Convert.ToUInt64(something))
      {
         A+=Convert.ToUInt64(i);
         found = true;
         break;
      }
      //Console.WriteLine($"{i} did not produce {something}");
   }
   if(!found) Console.WriteLine($"did not produce {something}");
}
*/
var res = find_a(0, program);
if(res.isValid)
{
   Console.WriteLine(res.result);
   Console.WriteLine("VALID");
}
else Console.WriteLine("invalid");

(ulong result, bool isValid) find_a(ulong A, int[] program) {
   if (program.Length == 0) {
      return (A, true);
   }
   var something = program[0];
   A<<=3;
   for(int i=0;i<8;i++)
   {
      var result = Logic(A+Convert.ToUInt64(i));
      if(result==Convert.ToUInt64(something))
      {
         A+=Convert.ToUInt64(i);
         var johonkin = find_a(A, program.Skip(1).ToArray());
         if(johonkin.isValid) {
            return johonkin;
         }
      }
   }
   return (0, false);
}

ulong Logic(ulong AA)
{
   ulong B = AA & 7;
   B ^= 2;
   ulong C = AA >> Convert.ToInt32(B);
   B ^= 3;
   B = B ^ C;
   return B&7;
   /*
   ulong B = (uint)(AA & 7);
   B ^= 2;
   ulong C = (uint)(AA >> Convert.ToInt32(B));
   B = B ^ C;
   B ^= 7;
   return B&7;
   */
}